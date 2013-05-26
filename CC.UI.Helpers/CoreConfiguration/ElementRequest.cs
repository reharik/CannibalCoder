using System;
using CC.Core.Utilities;
using Microsoft.Practices.ServiceLocation;

namespace CC.UI.Helpers.Configuration
{
    public class ElementRequest
    {
        private bool _hasFetched;
        private object _rawValue;

        public ElementRequest(object model, Accessor accessor)
        {
            Model = model;
            Accessor = accessor;
        }

        public string ElementId { get; set; }

        public object RawValue
        {
            get
            {
                if (!_hasFetched)
                {
                    _rawValue = Accessor.GetValue(Model);
                    _hasFetched = true;
                }
                return _rawValue;
            }
        }

        public object Model { get; private set; }

        public Accessor Accessor { get; private set; }

        public AccessorDef ToAccessorDef()
        {
            return new AccessorDef
                {
                    Accessor = Accessor,
                    ModelType = Model.GetType()
                };
        }

        public string StringValue()
        {
            if (RawValue == null || RawValue as string == string.Empty) return string.Empty;
            var type = RawValue.GetType();
            return type.IsNullable() ? type.GetInnerTypeFromNullable().ToString() : type.ToString();
        }
    }
}