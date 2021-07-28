namespace Lance.Common
{
    public delegate void ActionRef<T, TW>(ref T target, ref TW value);

    public delegate void ActionRef<T>(ref T target);

    public delegate TW FuncRef<T, TW>(ref T target);
}