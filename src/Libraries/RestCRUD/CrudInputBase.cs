namespace RestCRUD.Core
{
    using System;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    /// <summary>
    /// Extended version of the <see cref="InputBase{TValue}"/> allows for generated HTML ID attributes
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class CrudInputBase<TValue> : InputBase<TValue>
    {
        private string id = Guid.NewGuid().ToString();

        /// <summary>
        /// The html id attribute that could be used for the element
        /// </summary>
        [Parameter] public string Id { get => id; set => id = value; }
    }
}
