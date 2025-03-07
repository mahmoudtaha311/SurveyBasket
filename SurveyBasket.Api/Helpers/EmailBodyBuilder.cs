namespace SurveyBasket.Api.Helpers;

public static class EmailBodyBuilder
{
    public static string GenerateEmailBody(string template, Dictionary<string, string> templateModel)
    {
        var templatePath = $"{Directory.GetCurrentDirectory()}/Templates/{template}.html";
        var streamReader = new StreamReader(templatePath);
        var body = streamReader.ReadToEnd();
        streamReader.Close();
        foreach (var item in templateModel)
            body = body.Replace(item.Key, item.Value);

        return body;
    }
}
