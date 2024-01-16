using System.Collections;

public abstract class AIBaseState
{
    public AIState name;

    protected bool canUpdate;

    public abstract IEnumerator Enter(AIController ai);

    public abstract void Update(AIController ai);
}