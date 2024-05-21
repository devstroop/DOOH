# Usase

```html
<DOOH.Client.Components.UploadComponent @ref="uploadBrandLogo" OnComplete="@((value) => OnUploadComplete(value.FirstOrDefault()))" OnProgress="((value) => StateHasChanged())" Accept="image/*" />
```

```csharp
@code {
	private void OnUploadComplete(DOOH.Server.Models.DOOHDB.Attachment attachment)
	{
		// Do something with the uploaded file
	}
}
```