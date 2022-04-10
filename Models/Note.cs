using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text;

namespace NotesApi.Models;

public class Note
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    [BsonRequired]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Slug { 
        get {
            string phrase = $"{this.Title} {this.Id}";
            StringBuilder sb = new StringBuilder();
            bool wasHyphen = true;
            foreach (char c in phrase)
            {
                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(char.ToLower(c));
                    wasHyphen = false;
                }
                else 
                {
                    if (char.IsWhiteSpace(c) && !wasHyphen)
                    {
                        sb.Append('-');
                        wasHyphen = true;
                    }
                }
            }
            // Avoid trailing hyphens
            if (wasHyphen && sb.Length > 0)
            {
                sb.Length--;
            }
            return sb.ToString().Replace("--","-");
        }
    }
}
