using System;
using UnityEngine;

namespace blProject.scripts.EditorFramework
{
    public abstract class BaseModel : IModel
    {
        [Serialize]
        public int IModelInteger { get; set; } = 10;
        [Serialize]
        public int IModelIntegerWithoutSetter { get; } = 10;
        
        [Serialize]
        private int IModelIntegerPrivate = 10;
        
        [Serialize]
        private int BaseModelIntegerPrivate { get; set; } = 10;
        [Serialize]
        private int BaseModelIntegerWithoutSetterPrivate { get; } = 10;
        
        [Serialize]
        protected int BaseModelIntegerProtected { get; set; } = 10;
        [Serialize]
        protected int BaseModelIntegerWithoutSetterProtected { get; } = 10;

        public void IModelMethod()
        {
        }
    }
    
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class SerializeAttribute : Attribute
    {
        
    }
}