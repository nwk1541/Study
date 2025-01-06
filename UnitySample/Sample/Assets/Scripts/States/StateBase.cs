public abstract class StateBase
{
    protected CharacterBase _characterBase;

    protected StateBase(CharacterBase charBase)
    {
        _characterBase = charBase;
    }

    protected abstract void OnStateEnter();
    protected abstract void OnStateUpdate();
    protected abstract void OnStateLeave();
}
