[System.Serializable]
public class BaseComponent : IComponent
{
    protected BaseController Controller;

    public BaseComponent(BaseController controller)
    {
        Controller = controller;
    }

    public IController GetController()
    {
        return Controller;
    }
}
