using NotesApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace NotesApi.Services;

public class NotesService
{
    private readonly IMongoCollection<Note> _notesCollection;

    public NotesService(IOptions<NotesDatabaseSettings> notesDatabaseSettings)
    {
        var mongoClient = new MongoClient(notesDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(notesDatabaseSettings.Value.DatabaseName);
        _notesCollection = mongoDatabase.GetCollection<Note>(notesDatabaseSettings.Value.NotesCollectionName);
    }

    public async Task<List<Note>> GetAsync() =>
        await _notesCollection.Find(_ => true).ToListAsync();

    public async Task<Note?> GetAsync(string id) =>
        await _notesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Note newNote)
    {
        var now = DateTime.UtcNow;
        newNote.CreatedAt = now;
        newNote.UpdatedAt = now;
        await _notesCollection.InsertOneAsync(newNote);
    }

    public async Task UpdateAsync(string id, Note updatedNote) {
        var updateDefinition = Builders<Note>.Update
            .Set("Title", updatedNote.Title)
            .Set("Description", updatedNote.Description)
            .Set("Content", updatedNote.Content)
            .Set("UpdatedAt", DateTime.UtcNow);
        await _notesCollection.UpdateOneAsync(x => x.Id == id, updateDefinition);
    }

    public async Task RemoveAsync(string id) =>
        await _notesCollection.DeleteOneAsync(x => x.Id == id);
}
