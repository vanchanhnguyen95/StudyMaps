namespace BAGeocoding.Utility
{
    /// <summary>
    /// Interface của một Input
    /// </summary>
    public interface IInput
    {
        void Clear();
        void SetValue(object value);
        object GetValue();
    }
}
