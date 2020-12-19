namespace RestCRUD.Components
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Components.Forms;
    using RestCRUD;
    using RestCRUD.Core.Repository;
    using RestCRUD.Models;

    public class CustomComponentsRepository : FormGeneratorComponentsRepository
    {
        public CustomComponentsRepository()
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
