namespace blProject.scripts.EditorFramework
{
    public class TestModel : BaseModel
    {
        private int value = 10000;
        
        [Serialize]
        protected int IModelIntegerPrivate2 = 10;
    }
}