namespace AdsPortal.WebApi.Infrastructure.Identity.Jwt
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using AdsPortal.Application.Exceptions;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Operations.AuthenticationOperations.Queries.GetValidToken;
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Domain.Jwt;
    using AdsPortal.WebApi.Infrastructure.Identity.Configurations;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public class JwtService : IJwtService
    {
        private const int MINIMUM_JWT_LENGTH = 64;

        private readonly JwtConfiguration _settings;
        private readonly byte[] _key;
        private readonly SigningCredentials _signingCredentials;
        private readonly TokenValidationParameters _validationParameters;
        private readonly JwtSecurityTokenHandler _handler;

        public JwtService(IOptions<JwtConfiguration> settings)
        {
            _settings = settings.Value;
            string key = _settings.Key ?? throw new NullReferenceException("JWT key should not be null");

            if (key.Length < MINIMUM_JWT_LENGTH)
                throw new ArgumentException($"JWT key should be at least {MINIMUM_JWT_LENGTH} characters in Base64");

            _key = Base64UrlEncoder.DecodeBytes(key);

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha512Signature);

            _validationParameters = GetValidationParameters(_settings);

            _handler = new JwtSecurityTokenHandler();
        }

        public AuthenticateUserResponse GenerateJwtToken(IJwtUserData user)
        {
            return GenerateJwtToken(user, user.Role);
        }

        public AuthenticateUserResponse GenerateJwtToken(IJwtUserData user, Roles roles)
        {
            if (!user.IsActive)
                throw new ForbiddenException();

            ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), user.Id.GetType().Name),
                });

            //if (roles == Roles.None)
            //    throw new InvalidOperationException("None role not supported for token generation");

            //TODO: Fix User | Admin
            //if (!roles.IsDefined())
            //    throw new InvalidOperationException("Invalid roles");

            string[] rolesArray = roles.ToString()
                                       .Split(", ");

            foreach (string role in rolesArray)
                claims.AddClaim(new Claim(ClaimTypes.Role, role));

            DateTime utcNow = DateTime.UtcNow;
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = utcNow.Add(_settings.Lease),
                SigningCredentials = _signingCredentials,
                IssuedAt = utcNow,
                Issuer = _settings.Issuer ?? throw new NullReferenceException("Issuer should not be null")
            };
            SecurityToken result = _handler.CreateToken(tokenDescriptor);

            return new AuthenticateUserResponse
            {
                Token = _handler.WriteToken(result),
                Lease = _settings.Lease,
                ValidTo = result.ValidTo
            };
        }

        public void ValidateStringToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token is null or whitespace", nameof(token));

            _handler.ValidateToken(token, _validationParameters, out _);
        }

        public static TokenValidationParameters GetValidationParameters(JwtConfiguration settings)
        {
            byte[] key = Base64UrlEncoder.DecodeBytes(settings.Key);
            string issuers = settings.Issuer ?? throw new NullReferenceException("Issuer should not be null");

            //Issuer - who
            //Audience - to whom
            return new TokenValidationParameters
            {
                ValidateAudience = false,
                //AudienceValidator = DoValidation,
                ValidIssuers = new string[] { issuers },
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
                RequireSignedTokens = true
            };
        }

        public bool IsTokenStringValid(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            try
            {
                ValidateStringToken(token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Guid GetUserIdFromToken(string token)
        {
            JwtSecurityToken secToken = _handler.ReadJwtToken(token);
            Claim? claim = secToken.Claims?.FirstOrDefault(x => x.Type.Equals("nameidentifier") || x.Type.Equals(ClaimTypes.NameIdentifier));
            Guid userId = Guid.Parse(claim?.Value!);

            return userId;
        }

        public bool IsRoleInToken(string? token, Roles role)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            if (!role.IsDefined())
                return false;

            JwtSecurityToken jwtToken = _handler.ReadJwtToken(token);
            List<Claim> claims = jwtToken.Claims.Where(x => x.Type.Equals("role") || x.Type.Equals(ClaimTypes.Role)).ToList();

            return claims.FirstOrDefault(x => x.Value.Equals(role)) != null;
        }

        public bool IsAnyOfRolesInToken(string? token, Roles roles)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            if (!roles.IsDefined())
                return false;

            JwtSecurityToken jwtToken = _handler.ReadJwtToken(token);
            List<Claim> claims = jwtToken.Claims.Where(x => x.Type.Equals("role") || x.Type.Equals(ClaimTypes.Role)).ToList();

            string[] rolesArray = roles.ToString()
                                       .Split(", ");

            IEnumerable<string> intersection = claims.Select(x => x.Value)
                                                     .Intersect(rolesArray);

            return intersection.Any();
        }
    }
}
