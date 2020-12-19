namespace RestCRUD.Repository.Plain
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Components.Forms;
    using RestCRUD.Core;
    using RestCRUD.Core.Repository;
    using RestCRUD.Form.Components.Plain;
    using RestCRUD.Models;

    public class VxComponentsRepository : FormGeneratorComponentsRepository
    {
        public VxComponentsRepository()
        {
            _ComponentDict = new Dictionary<Type, Type>()
                {
                    {typeof(string), typeof(VxInputText) },
                    {typeof(DateTime), typeof(InputDate<>) },
                    {typeof(int), typeof(InputNumber<>) },
                    {typeof(bool), typeof(VxInputCheckbox) },
                    {typeof(Enum), typeof(InputSelectWithOptions<>) },
                    {typeof(ValueReferences), typeof(InputCheckboxMultiple<>) },
                    {typeof(decimal), typeof(InputNumber<>) },
                    {typeof(VxColor), typeof(InputColor) }
                };

            _DefaultComponent = null;
        }
    }
}
