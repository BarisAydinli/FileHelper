namespace FileHelper;
#pragma warning disable
public class EntityDal<TEntity> where TEntity : class, new()
{
    private readonly string _path;
    private StreamWriter streamWriter;
    public EntityDal(string path) => _path = path;

    public void Add(TEntity entity)
    {
        var entities = GetAll();
        entities.Add(entity);

        streamWriter = new StreamWriter(_path, false);
        List<string> entityList = new List<string>();

        ConvertToCsv(entity, entities, entityList);

        entityList.ForEach(e => streamWriter.WriteLine(e));
        streamWriter.Close();
    }

    public void Delete(TEntity entity)
    {
        var entities = GetAll();

        var x = (string)entity.GetType().GetProperties()[0].GetValue(entity);
        for (int i = 0; i < entities.Count; i++)
        {
            var y = (string)entities[i].GetType().GetProperties()[0].GetValue(entities[i]);
            if (x == y) entities.RemoveAt(i);
        }

        List<string> entityList = new List<string>();
        StreamWriter streamWriter = new StreamWriter(_path, false);

        ConvertToCsv(entity, entities, entityList);

        entityList.ForEach(e => streamWriter.WriteLine(e));
        streamWriter.Close();
    }
    public void Update(TEntity entity)
    {
        var entities = GetAll();

        var x = (string)entity.GetType().GetProperties()[0].GetValue(entity);
        for (int i = 0; i < entities.Count; i++)
        {
            var y = (string)entities[i].GetType().GetProperties()[0].GetValue(entities[i]);
            if (x == y)
            {
                entities.RemoveAt(i);
                entities.Insert(i, entity);
            }
        }

        List<string> entityList = new List<string>();
        StreamWriter streamWriter = new StreamWriter(_path, false);

        ConvertToCsv(entity, entities, entityList);

        entityList.ForEach(e => streamWriter.WriteLine(e));
        streamWriter.Close();
    }
    public TEntity Get(TEntity entity)
    {
        var entities = GetAll();
        var x = (string)entity.GetType().GetProperties()[0].GetValue(entity);
        for (int i = 0; i < entities.Count; i++)
        {
            var y = (string)entities[i].GetType().GetProperties()[0].GetValue(entities[i]);
            if (x == y) return entities[i];
        }
        return null;
    }
    public List<TEntity> GetAll()
    {
        if (!File.Exists(_path)) File.Create(_path);
        var lines = File.ReadAllLines(_path);
        var entities = new List<TEntity>();
        var entity = new TEntity();
        var properties = entity.GetType().GetProperties();
        foreach (var line in lines)
        {
            var values = line.Split(';');
            for (int i = 0; i < properties.Length; i++) properties[i].SetValue(entity, values[i]);
            entities.Add(entity);
            entity = new TEntity();
        }
        return entities;
    }
    private static void ConvertToCsv(TEntity entity, List<TEntity> entities, List<string> entityList)
    {
        var properties = entity.GetType().GetProperties();
        for (int i = 0; i < entities.Count; i++)
        {
            string entityStr = "";
            string finalEntity = "";
            for (int j = 0; j < properties.Length; j++) entityStr += $"{properties[j].GetValue(entities[i])};";
            char[] entityCa = entityStr.ToCharArray();
            for (int k = 0; k < entityCa.Length - 1; k++) finalEntity += entityCa[k];
            entityList.Add(finalEntity);
        }
    }
}