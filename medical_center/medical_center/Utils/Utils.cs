using System.IO;
using System.Web;

namespace MED.Presentation.Utils
{
    public class Utils
    {
        public static byte[] ImageToByte(HttpPostedFileBase img)
        {
            byte[] data;
            using (var inputStream = img.InputStream)
            {
                if (!(inputStream is MemoryStream memoryStream))
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }
            return data;

        }
    }
}
