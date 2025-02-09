public partial class ConnectBase
{
    protected void OnInitialized()
    {
        // do stuff
    }

    private async void SendMail()
    {
        emailRequest.Body = JsonConvert.SerializeObject(form.Model);
        emailRequest.Subject = "Inquiry from DevEddie.com";
        emailRequest.ToEmail = "eddie.waked+inquiries@gmail.com";

        try
        {
            var response = await HttpClient.PostAsJsonAsync("SendEmail", emailRequest);

            if (response.IsSuccessStatusCode)
            {
                statusMessage = "Email sent successfully!";
                contact.ConnectionType = null;
                contact.Email = null;
                contact.Message = null;
                contact.Name = null;
            }
            else
            {
                statusMessage = $"Failed to send email. Status: {response.StatusCode}";
            }

            StateHasChanged();
        }
        catch (Exception ex)
        {
            statusMessage = $"Error: {ex.Message}";
        }
    }
}