
namespace Chat.api.Model
{
    public class SelfLink
    {
        public readonly string self;
        internal SelfLink(string linkUri, int id)
        {
            self = $"{linkUri}/{id}";
        }
    }
}