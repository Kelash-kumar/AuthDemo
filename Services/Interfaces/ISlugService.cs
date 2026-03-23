namespace AuthDemo.Services.Interfaces
{
    public interface ISlugService
    {
        //Converts a string to a URL-friendly slug. e.g. "Hello World!" → "hello-world"
        string Generate(string input);

        //Generates a unique slug by checking the DB table. Appends -2, -3 if taken.
        string GenerateUnique(string input, IEnumerable<string> existingSlugs);
    }


}
