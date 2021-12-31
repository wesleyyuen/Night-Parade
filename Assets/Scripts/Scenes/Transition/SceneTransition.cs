public interface ISceneTransition
{
    float TransitionDuration
    {
        get;
    }
    
    void StartSceneTransitionIn();
    void StartSceneTransitionOut(string levelToLoad, ref PlayerData playerVariables);
}