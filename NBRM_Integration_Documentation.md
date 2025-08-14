# NBRM Web Service Integration - Internship Project

## 📋 Project Overview

**Task (Macedonian):** Во c# да се направи конзолно програмче или графичко со кое ќе се повика веб сервисот на народна банка за да врати курсна листа

**Task (English):** Create a C# console or graphical application that calls the National Bank web service to return exchange rate list

**Solution:** ASP.NET Core MVC Web Application with SOAP Web Service Integration

## 🏛️ National Bank Web Service

- **Bank:** Народна Банка на Република Северна Македонија (NBRM)
- **Service URL:** https://www.nbrm.mk/KLServiceNOV/en
- **WSDL Endpoint:** https://www.nbrm.mk/KLServiceNOV/KLService.asmx
- **Type:** SOAP Web Service

## 🛠️ Technical Implementation

### Architecture
```
┌─────────────────┐    ┌──────────────────┐    ┌─────────────────┐
│   Web Browser   │───▶│  ASP.NET Core    │───▶│ NBRM SOAP       │
│   (User UI)     │    │  MVC Controller  │    │ Web Service     │
└─────────────────┘    └──────────────────┘    └─────────────────┘
```

### Key Components

#### 1. **SOAP Service Client** (`Services/NBRMServiceClient.cs`)
- **Purpose:** Handles SOAP communication with NBRM web service
- **Methods:**
  - `GetExchangeRatesAsync()` - Current exchange rates
  - `GetExchangeRatesDAsync(string date)` - Historical rates for specific date
- **Features:**
  - XML SOAP envelope construction
  - Response parsing and error handling
  - Proper resource disposal (IDisposable)

#### 2. **Data Models**
- **`ExchangeRateRow.cs`** - Represents individual currency exchange rate
- **`ExchangeRateResponse.cs`** - Wrapper for service response with metadata

#### 3. **MVC Controller** (`Controllers/HomeController.cs`)
- **Route:** `/Home/Index`
- **Parameters:**
  - `selectedDate` - Date filter (yyyy-MM-dd format)
  - `searchCurrency` - Currency/country filter
- **Features:**
  - Async/await pattern
  - Error handling with logging
  - Data filtering and search

#### 4. **User Interface** (`Views/Home/Index.cshtml`)
- **Design:** Responsive Bootstrap 5 layout
- **Languages:** Bilingual (Macedonian/English)
- **Features:**
  - Date picker for historical rates
  - Currency search functionality
  - Sortable exchange rates table
  - Error messages and loading states

## 📦 NuGet Packages

```xml
<PackageReference Include="System.ServiceModel.Http" Version="6.0.0" />
<PackageReference Include="System.ServiceModel.Primitives" Version="6.0.0" />
<PackageReference Include="System.ServiceModel.Duplex" Version="6.0.0" />
<PackageReference Include="System.ServiceModel.Security" Version="6.0.0" />
<PackageReference Include="Newtonsoft.Json" Version="13.0.1" />
```

## 🚀 How to Run

1. **Prerequisites:**
   - .NET 8.0 SDK
   - Visual Studio 2022 or VS Code
   - Internet connection (for NBRM service)

2. **Build and Run:**
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

3. **Access Application:**
   - Open browser to: `https://localhost:5001` or `http://localhost:5000`

## 🎯 Features Demonstrated

### ✅ Core Requirements
- [x] **C# Application** - ASP.NET Core MVC
- [x] **Web Service Integration** - NBRM SOAP service
- [x] **Exchange Rate Retrieval** - Current and historical data
- [x] **Graphical Interface** - Modern web UI

### ✅ Additional Features
- [x] **Bilingual Support** - Macedonian and English
- [x] **Date Filtering** - Historical exchange rates
- [x] **Search Functionality** - Filter by currency/country
- [x] **Error Handling** - Graceful error management
- [x] **Responsive Design** - Mobile-friendly interface
- [x] **Professional UI** - Bootstrap styling with modern design

## 📊 Data Structure

### NBRM Response Format
```xml
<soap:Envelope>
  <soap:Body>
    <GetExchangeRatesResponse>
      <GetExchangeRatesResult>
        <KursnaLista>
          <Sifra>USD</Sifra>
          <Zemja>САД</Zemja>
          <Nominal>1</Nominal>
          <Kurs>55.3500</Kurs>
        </KursnaLista>
        <!-- More currencies... -->
      </GetExchangeRatesResult>
    </GetExchangeRatesResponse>
  </soap:Body>
</soap:Envelope>
```

### Application Data Model
```csharp
public class ExchangeRateRow
{
    public string Code { get; set; }        // USD, EUR, etc.
    public string Country { get; set; }     // САД, Германија, etc.
    public decimal Nominal { get; set; }    // 1, 100, etc.
    public decimal Rate { get; set; }       // 55.3500, etc.
    public DateTime Date { get; set; }      // Rate date
}
```

## 🔧 SOAP Integration Details

### Request Example
```xml
POST https://www.nbrm.mk/KLServiceNOV/KLService.asmx
Content-Type: text/xml; charset=utf-8
SOAPAction: http://www.nbrm.mk/GetExchangeRates

<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <GetExchangeRates xmlns="http://www.nbrm.mk/" />
  </soap:Body>
</soap:Envelope>
```

### Error Handling
- Network connectivity issues
- SOAP fault responses
- XML parsing errors
- Service unavailability

## 📝 Testing Scenarios

1. **Current Rates:** Load application → View today's exchange rates
2. **Historical Rates:** Select past date → View historical rates
3. **Currency Search:** Enter "USD" or "Америка" → Filtered results
4. **Error Handling:** Disconnect internet → Graceful error message
5. **Date Validation:** Select future date → Appropriate handling

## 🎓 Learning Outcomes

### Technical Skills Demonstrated
- **SOAP Web Services** - Legacy service integration
- **XML Processing** - Parsing complex SOAP responses
- **Async Programming** - Modern async/await patterns
- **Error Handling** - Robust exception management
- **MVC Architecture** - Proper separation of concerns
- **Responsive Design** - Mobile-first web development

### Banking Domain Knowledge
- **Exchange Rates** - Understanding currency conversion
- **Central Banking** - National bank data services
- **Financial Data** - Real-time vs historical data

## 🌐 URLs and Resources

- **NBRM Official Site:** https://www.nbrm.mk
- **Web Service Info:** https://www.nbrm.mk/web-servis-novo-en.nspx
- **Service Endpoint:** https://www.nbrm.mk/KLServiceNOV/en
- **WSDL:** https://www.nbrm.mk/KLServiceNOV/KLService.asmx?WSDL

## 📄 Project Structure

```
WebApplicationIntern2/
├── Controllers/
│   └── HomeController.cs          # MVC Controller
├── Models/
│   ├── ExchangeRateRow.cs         # Currency data model
│   └── ExchangeRateResponse.cs    # Response wrapper
├── Services/
│   ├── INBRMService.cs            # Service interface
│   └── NBRMServiceClient.cs       # SOAP client implementation
├── Views/
│   └── Home/
│       └── Index.cshtml           # Main UI page
├── Program.cs                     # Application startup
└── WebApplicationIntern2.csproj   # Project configuration
```

---

**Created by:** Senior Developer Assistant  
**Date:** December 2024  
**Purpose:** Internship Task Completion Documentation  
**Status:** ✅ COMPLETED SUCCESSFULLY
