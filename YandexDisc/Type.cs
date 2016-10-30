using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace YandexDisc
{
    [DataContract]
    class TypeDisc
    {
        [DataMember(Name = "trash_size")]
        public int trash_size { get; set; }
        [DataMember(Name = "total_space")]
        public double total_space { get; set; }
        [DataMember(Name = "used_space")]
        public double used_space { get; set; }
        [DataMember(Name = "system_folders")]
        //List<TypeSystemFolders> 
        public TypeSystemFolders system_folders { get; set; }
}
    [DataContract]
    class TypeSystemFolders
    {
        [DataMember(Name = "applications")]
        public string applications { get; set; }
        [DataMember(Name = "downloads")]
        public string downloads { get; set; }
    }

    /*
    Resource {
        public_key (string, optional): <Ключ опубликованного ресурса>,
        public_url (string, optional): <Публичный URL>,
        _embedded (ResourceList, optional): <Список вложенных ресурсов>,
        name (string): <Имя>,
        created (string): <Дата создания>,
        deleted (string, optional): <Дата добавления в корзину(для ресурсов в корзине)>,
        custom_properties (object, optional): <Пользовательские атрибуты ресурса.>,
        origin_path (string, optional): <Путь откуда был удалён ресурс>,
        modified (string): <Дата изменения>,
        md5 (string, optional): <MD5-хэш>,
        path (string): <Путь к ресурсу>,
        media_type (string, optional): <Определённый Диском тип файла>,
        preview (string, optional): <URL превью файла>,
        type (string): <Тип>,
        mime_type (string, optional): <MIME-тип файла>,
        size (integer, optional): <Размер файла>
    }
    ResourceList {
        sort (string, optional): <Поле, по которому отсортирован список>,
        items (array[Resource]): <Элементы списка>,
        limit (integer, optional): <Количество элементов на странице>,
        offset (integer, optional): <Смещение от начала списка>,
        path (string): <Путь к ресурсу, для которого построен список>,
        total (integer, optional): <Общее количество элементов в списке>
    }
    */
    [DataContract]
    public class TypeResource
    {
        [DataMember(Name = "public_key")]
        public string public_key { get; set; }
        [DataMember(Name = "public_url")]
        public string public_url  { get; set; }
        [DataMember(Name = "_embedded")]
        public TypeResourceList _embedded { get; set; }
        [DataMember(Name = "name")]
        public string name { get; set; }
        [DataMember(Name = "created")]
        public string created { get; set; }
        [DataMember(Name = "deleted")]
        public string deleted { get; set; }
        [DataMember(Name = "custom_properties")]
        public string custom_properties { get; set; }
        [DataMember(Name = "origin_path")]
        public string origin_path { get; set; }
        [DataMember(Name = "modified")]
        public string modified { get; set; }
        [DataMember(Name = "md5")]
        public string md5 { get; set; }
        [DataMember(Name = "path")]
        public string path { get; set; }
        [DataMember(Name = "media_type")]
        public string media_type { get; set; }
        [DataMember(Name = "preview")]
        public string preview { get; set; }
        [DataMember(Name = "type")]
        public string type { get; set; }
        [DataMember(Name = "mime_type")]
        public string mime_type { get; set; }
        [DataMember(Name = "size")]
        public int size { get; set; }
    }

    [DataContract]
    public class TypeResourceList
    {
        [DataMember(Name = "public_key")]
        public string public_key { get; set; }
        [DataMember(Name = "items")]
        public TypeResource[] items { get; protected set; }
        [DataMember(Name = "limit")]
        public int limit { get; set; }
        [DataMember(Name = "offset")]
        public int offset { get; set; }
        [DataMember(Name = "path")]
        public string path { get; set; }
        [DataMember(Name = "total")]
        public int total { get; set; }
    }

    [DataContract]
    public class TypeLink
    {
        [DataMember(Name = "href")]
        public string href { get; set; }
        [DataMember(Name = "method")]
        public string method { get; set; }
        [DataMember(Name = "templated")]
        public bool templated { get; set; }

    }

}
