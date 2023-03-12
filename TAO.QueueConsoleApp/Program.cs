using System.Text;
using TAO.AzureStorage.Services.Concrete;

TAO.AzureStorage.ConnectionStrings.AzureStorageConnectionsString = "DefaultEndpointsProtocol=https;AccountName=moongazingstorageaccount;AccountKey=fvKc4w29PDGhsKExHksbPgtH/4lIseAZ2FY+LDdNNcd6r1nyfzR7TAaA1fK/pv4jGTzVg/AdErEb+AStrkF69Q==;EndpointSuffix=core.windows.net";

AzQueue queue = new("example");

int operation;

Console.WriteLine("Select Operation: \n 1-) Send Message \n 2-) Read Message \n 3-) Delete Message");
operation = Convert.ToInt32(Console.ReadLine());
if(operation == 1)
{
    Console.Clear();

    Console.Write("Enter your message:");
   
    string base64Message = Convert.ToBase64String(Encoding.UTF8.GetBytes(Console.ReadLine()));

    await queue.SendMessageAsync(base64Message);

    Console.WriteLine("Message will be send.");
}
else if(operation == 2)
{
    Console.Clear();

    var message = queue.RetrieveNextMessageAsync().Result;

    string text = Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageText));

    Console.WriteLine("Message:" + text);
}
else
{
    Console.Clear();

    var message = queue.RetrieveNextMessageAsync().Result;

    await queue.DeleteMessageAsync(message.MessageId,message.PopReceipt);

    Console.WriteLine("Message deleted.");
}










