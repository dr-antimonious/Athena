using Artemis.Contracts.Entities.Interfaces;

namespace Artemis.Contracts
{
    public static class GenericListExtension
    {
        public static List<T1> Convert<T1, T2>(this List<T2> list)
            where T2 : IConvertable<T1>
        {
            List<T1> result = new();

            foreach (T2 shot in list)
                result.Add(shot.Convert());
            
            return result;
        }
    }
}
