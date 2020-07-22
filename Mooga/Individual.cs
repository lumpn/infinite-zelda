namespace Lumpn.Mooga
{
    public interface Individual
    {
        Genome Genome { get; }

        double GetScore(int attribute);
        void SetScore(int attribute, double value);
    }
}
