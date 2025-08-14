# Visual Studio 2022 Connected Services Guide

## 🎯 Adding NBRM Web Service via Connected Services

### **Step 1: Open Connected Services**
1. **Right-click** on your project `WebApplicationIntern2` in Solution Explorer
2. **Select** `Add` → `Connected Service`
3. **Choose** `Microsoft WCF Web Service Reference Provider`

### **Step 2: Configure Web Service Reference**
1. **URL/Path:** `https://www.nbrm.mk/KLServiceNOV/KLService.asmx?WSDL`
2. **Namespace:** `NBRMWebService`
3. **Reference Name:** `NBRMService`
4. **Click** `Go` to load the service
5. **Click** `Next` and then `Finish`

### **Step 3: What Gets Generated**
Visual Studio will create:
- `Connected Services/NBRMService/` folder
- `Reference.cs` with service client classes
- Service configuration in `appsettings.json`

### **Step 4: Update Controller Code**

Replace the TODO section in `HomeController.cs` with:

```csharp
// Use the generated service client
var client = new NBRMWebService.KLServiceSoapClient();

try 
{
    // Call the web service method
    var result = await client.GetExchangeRatesAsync();
    
    // Parse the result into your ExchangeRateRow list
    var exchangeRates = ParseNBRMResult(result);
    
    // Apply date filter if needed
    if (!string.IsNullOrEmpty(selectedDate))
    {
        // Use GetExchangeRatesDAsync for specific date
    }
    
    model.ExchangeRates = exchangeRates;
}
finally
{
    if (client.State == System.ServiceModel.CommunicationState.Opened)
        await client.CloseAsync();
}
```

### **Step 5: Add Parser Method**

Add this method to your HomeController:

```csharp
private List<ExchangeRateRow> ParseNBRMResult(/* result type from service */)
{
    var rates = new List<ExchangeRateRow>();
    
    // Add MKD base currency
    rates.Add(new ExchangeRateRow 
    { 
        Code = "MKD", 
        Country = "North Macedonia", 
        Currency = "Macedonian Denar", 
        Nominal = 1, 
        Rate = 1.0000m,
        TrendIcon = "📈"
    });
    
    // Parse service result and add to rates list
    // Implementation depends on the actual service response structure
    
    return rates;
}
```

## ✅ **Advantages of Connected Services Approach**

1. **Professional Standard** - Industry best practice
2. **Type Safety** - Strongly typed service client
3. **IntelliSense Support** - Full IDE support
4. **Error Handling** - Built-in SOAP fault handling
5. **Configuration** - Proper service endpoints in config
6. **Maintainable** - Easy to update when service changes

## 🔧 **What Your Mentor Expects**

Your mentor wants to see:
- Proper Visual Studio project structure
- Generated service reference files
- Professional SOAP client usage
- Enterprise-level code organization

## 📝 **Current Status**

- ✅ Manual HTTP calls removed
- ✅ Sample data showing the structure
- ⏳ **Next Step:** Add Connected Service in Visual Studio
- ⏳ **Then:** Replace sample data with actual service calls

This approach demonstrates proper enterprise development practices that are expected in professional environments.
