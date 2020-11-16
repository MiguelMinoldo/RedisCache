using System;

namespace Foundation.DI
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ServiceAttribute : Attribute
    {
        public ServiceAttribute(Type serviceType)
        {
            this.ServiceType = serviceType;
        }

        public Lifetime Lifetime { get; set; } = Lifetime.Transient;

        public Type ServiceType { get; set; }
    }
}
