using System;

namespace Chat.api.Model
{
    public class JsonApiPagination
    {
        public readonly string self;
        public readonly string first;
        public readonly string prev;
        public readonly string next;
        public readonly string last;
        public JsonApiPagination(Uri requestUri, int start, int size, int count)
        {
            self = requestUri.ToString();
            var basePath = string.IsNullOrEmpty(requestUri.Query) ? self : self.Substring(0, self.IndexOf("?"));
            first = $"{basePath}?page[number]=1&page[size]={size}";
            prev = start < size ? null : $"{basePath}?page[number]={start - size}&page[size]={size}";
            next = start + size > count ? null : $"{basePath}?page[number]={start + size}&page[size]={size}";
            var lastStart = count < size ? 1 : count - size;
            last = $"{basePath}?page[number]={lastStart}&page[size]={size}";
        }
    }
}