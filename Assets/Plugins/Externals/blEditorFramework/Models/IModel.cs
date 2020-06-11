
namespace blProject.scripts.EditorFramework
{
    [Serialize]
    public interface IModel
    {
        int IModelInteger { get; set; }
        int IModelIntegerWithoutSetter { get; }
        void IModelMethod();
    }
}