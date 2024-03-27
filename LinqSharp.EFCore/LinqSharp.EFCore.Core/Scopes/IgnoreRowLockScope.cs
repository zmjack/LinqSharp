using LinqSharp.EFCore.Design;
using NStandard;

namespace LinqSharp.EFCore.Scopes;

public class IgnoreRowLockScope : Scope<IgnoreRowLockScope>
{
    private bool _origin;
    private IRowLockable _lockable;

    public IgnoreRowLockScope(IRowLockable lockable, bool origin)
    {
        lockable.IgnoreRowLock = true;
        _lockable = lockable;
        _origin = origin;
    }

    public override void Disposing()
    {
        _lockable.IgnoreRowLock = _origin;
    }
}
