using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;

namespace United.Mobile.DataAccess.DynamoDB
{
    public class CatalogDynamoDB
    {
        private readonly IConfiguration _configuration;
        private readonly IDynamoDBService _dynamoDBService;
        public CatalogDynamoDB(IConfiguration configuration, IDynamoDBService dynamoDBService)
        {
            _configuration = configuration;
            _dynamoDBService = dynamoDBService;
        }

        public async Task<T> GetCatalogItems<T>(string itemId, string sessionId)
        {
            try
            {
                return await _dynamoDBService.GetRecords<T>(_configuration?.GetValue<string>("DynamoDBTables:uatb_Catalog"), "catalog01", itemId, sessionId);
            }
            catch
            {

            }

            return default;
        }
        public async Task<T> GetABTestingList<T>(string applicationID, string mpAccountNumber, string appVersion, string sessionId)
        {
            #region
            //Database database = DatabaseFactory.CreateDatabase("ConnectionString - iPhone");
            //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("uasp_Select_ABTesting_Items");
            //database.AddInParameter(dbCommand, "@ApplicationId", DbType.Int32, request.Application.Id);
            //database.AddInParameter(dbCommand, "@MPAccountNumber", DbType.String, request.MPAccountNumber);
            //database.AddInParameter(dbCommand, "@AppVersion", DbType.String, request.Application.Version.Major);

            //            USE[iPhone]
            //GO
            ///****** Object: StoredProcedure [dbo].[uasp_Select_ABTesting_Items] Script Date: 08-11-2021 23:25:46 ******/
            //SET ANSI_NULLS ON
            //GO
            //SET QUOTED_IDENTIFIER ON
            //GO
            //ALTER PROCEDURE[dbo].[uasp_Select_ABTesting_Items]
            //@ApplicationId INT,
            //@MPAccountNumber nvarchar(64),
            //@AppVersion nvarchar(16) = ''
            //AS
            //BEGIN
            //SET NOCOUNT ON;

            //            SELECT A.AB_Switch,A.MP_Flow as AB_FLOW,A.Default_Flow into #tmp
            //from uatb_ABTesting_AppFlow_Check (NOLOCK)A
            //left join uatb_ABTesting_MP_List(NOLOCK) B on B.AB_ID = A.AB_ID
            //WHERE A.APP_ID = @ApplicationId and B.MPAccountNumber = @MPAccountNumber
            //if ((select COUNT(*) from #tmp)= 0)
            //BEGIN
            // insert into #tmp
            //SELECT distinct A.AB_Switch,A.Default_Flow as AB_FLOW,A.Default_Flow
            //from uatb_ABTesting_AppFlow_Check(NOLOCK) A
            //left join uatb_ABTesting_MP_List(NOLOCK) B on B.AB_ID = A.AB_ID
            //WHERE A.APP_ID = @ApplicationId
            //END
            //select* from #tmp
            //drop table #tmp
            //END
            #endregion
            try
            {
                string key = mpAccountNumber + "::" + applicationID;
                return await _dynamoDBService.GetRecords<T>(_configuration?.GetValue<string>("DynamoDBTables:uatb_ABTesting_AppFlow_Check"), "abtesting01", key, sessionId);
            }
            catch 
            {
            }

            return default;
        }
        public async Task<T> CheckBetaAppUserForceUpdateByMPNumber<T>(string mileagePlusNumber, string applicationID, string appVersion, string deviceID, string sessionId)
        {
            #region
            //Database database = DatabaseFactory.CreateDatabase("ConnectionString - iPhone");
            //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("uasp_CheckBetaAppUserForceUpdateByMPNumber");
            //database.AddInParameter(dbCommand, "@MileagePlusNumber", DbType.String, request.MileagePlusID);
            //database.AddInParameter(dbCommand, "@ApplicationID", DbType.Int32, request.Application.Id);
            //database.AddInParameter(dbCommand, "@AppVersion", DbType.String, request.Application.Version.Major.ToUpper().Trim().TrimEnd('I').TrimEnd('A'));
            //database.AddInParameter(dbCommand, "@DeviceID", DbType.String, request.DeviceId);

            //            USE[iPhone]
            //GO
            ///****** Object: StoredProcedure [dbo].[uasp_CheckBetaAppUserForceUpdateByMPNumber] Script Date: 08-11-2021 23:28:12 ******/
            //SET ANSI_NULLS ON
            //GO
            //SET QUOTED_IDENTIFIER ON
            //GO
            //ALTER PROCEDURE[dbo].[uasp_CheckBetaAppUserForceUpdateByMPNumber]
            //-- Add the parameters for the stored procedure here
            //@MileagePlusNumber nVarchar(20),
            //@ApplicationID int,
            //@AppVersion nVarchar(10),
            //@DeviceID nVarchar(128) = null
            //AS
            //BEGIN
            //-- Output Variable Declaration
            //Declare @DisplayMessage nVarchar(1000),
            //@CTARemindMe nVarchar(50),
            //@CTAUpdateNow nVarchar(50),
            //@CTAOK nVarchar(50),
            //@RemindMeInterval_Hours int,
            //@RemindMeInterval_DateTime DateTime,
            //@LogRemindMe_UpdateNow_Message int = 0,
            //--Variable Declaration
            //@MessageForceUpdateAppVersion nVarchar(500),
            //@StrUpdateNow nVarchar(50),
            //@IsInBetaFlag int = 0




            //Declare @BetaFeatureMPAccess table
            //(IsInBeta varchar(10),
            //OptOutEligible varchar(10),
            //OptOutMileagePlusAccount varchar(10),
            //OptOutDevice varchar(10)
            //)



            //-- Variable value assignment
            //Select @MessageForceUpdateAppVersion = LegalDocument from uatb_DocumentLibrary Nolock Where Title = 'BETAApp_ForceUpdateMessage'
            //Select @StrUpdateNow = LegalDocument from uatb_DocumentLibrary Nolock Where Title = 'AppReleaseUpdates_CTAUpdateNow'

            //-- Core Logic
            //------------------
            //insert into @BetaFeatureMPAccess
            //Exec uasp_Select_BetaFeatureMPAccess @MileagePlusNumber, '1', @ApplicationID, @AppVersion, @DeviceID



            //Select top 1 @IsInBetaFlag = IsInBeta from @BetaFeatureMPAccess




            //--If((select count(*) from[uatb_BetaTester] Nolock Where MileagePlusNumber = @MileagePlusNumber) > 0 )
            //If(@IsInBetaFlag = 1)
            //Begin
            //-- Fetch and assigned to the variable for the most recent active entry from the uatb_AppReleaseUpdates table
            //Set @DisplayMessage = @MessageForceUpdateAppVersion
            //Set @CTAUpdateNow = @StrUpdateNow
            //Set @RemindMeInterval_Hours = 0
            //Set @RemindMeInterval_DateTime = ''
            //End

            //Select @DisplayMessage as 'DisplayMessage',
            //@CTARemindMe as 'CTARemindMe',
            //@CTAUpdateNow as 'CTAUpdateNow',
            //@CTAOK as 'CTAOK',
            //@RemindMeInterval_Hours as 'RemindMeInterval_Hours',
            //@RemindMeInterval_DateTime as 'RemindMeInterval_DateTime',
            //@LogRemindMe_UpdateNow_Message as 'LogRemindMe_UpdateNow_Message'
            //END
            #endregion
            try
            {
                string key = mileagePlusNumber + "::" + deviceID;
                return await _dynamoDBService.GetRecords<T>(_configuration?.GetValue<string>("DynamoDBTables:uatb_DocumentLibrary"), "forceUpdate01", key, sessionId);
            }
            catch
            {

            }

            return default;
        }
        public async Task<T> GetReleaseUpdateDetail<T>(string applicationID, string osVersion, string appVersion, string osRemindMeCounter, string appRemindMeCounter, string sessionId)
        {
            #region
            //Database database = DatabaseFactory.CreateDatabase("ConnectionString - iPhone");
            //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("uasp_GetAppReleaseUpdatesDetails");
            //database.AddInParameter(dbCommand, "@ApplicationId", DbType.Int32, request.Application.Id);
            //database.AddInParameter(dbCommand, "@OSVersion", DbType.String, request.OSVersion);
            //database.AddInParameter(dbCommand, "@AppVersion", DbType.String, request.Application.Version.Major);
            //database.AddInParameter(dbCommand, "@OSRemindMeCounter", DbType.Int32, request.OSVersionCounter);
            //database.AddInParameter(dbCommand, "@AppRemindMeCounter", DbType.Int32, request.AppVersionCounter);
            //            USE[iPhone]
            //GO
            ///****** Object: StoredProcedure [dbo].[uasp_GetAppReleaseUpdatesDetails] Script Date: 08-11-2021 23:33:54 ******/
            //SET ANSI_NULLS ON
            //GO
            //SET QUOTED_IDENTIFIER ON
            //GO
            //-- =========================================
            //--Table Name: uatb_GetReleaseUpdate
            //-- Created By: v761724(Nizam)
            //-- Created On: 06 / 12 / 2018
            //-- =========================================
            //ALTER PROCEDURE[dbo].[uasp_GetAppReleaseUpdatesDetails]--1, '0', '1.1.49', 0, 0, @DisplayMessage output,0,0,0,0,0
            //-- Add the parameters for the stored procedure here
            //@ApplicationID int,
            //--@ApplicationName varchar(10),
            //@OSVersion nVarchar(10),
            //@AppVersion nVarchar(10),
            //@OSRemindMeCounter int,
            //@AppRemindMeCounter int
            //AS
            //BEGIN
            //-- Output Variable Declaration
            //Declare @DisplayMessage nVarchar(1000),
            //@CTARemindMe nVarchar(50),
            //@CTAUpdateNow nVarchar(50),
            //@CTAOK nVarchar(50),
            //@RemindMeInterval_Hours int,
            //@RemindMeInterval_DateTime DateTime,
            //@LogRemindMe_UpdateNow_Message int,

            //--Variable Declaration
            //@MessageGracefulAppVersion nVarchar(500),
            //@MessageForceUpdateAppVersion nVarchar(500),
            //@MessagePostGraceAppVersion nVarchar(500),
            //@MessageOSVersion nVarchar(500),
            //@StrRemindMe nVarchar(50),
            //@StrUpdateNow nVarchar(50),
            //--@StrOK nVarchar(50),
            //@SupportOSVersion varchar(10),
            //@SupportAppVersion nVarchar(10),
            //@StartDate Date,
            //@EndDate Date,
            //@GracefulAppRemindMeInterval_Hours Int,
            //@GracefulAppRemindMeCount Int,
            //@PostGraceAppRemindMeInterval_Hours Int,
            //@PostGraceAppRemindMeCount Int,
            //@OSRemindMeInterval_Hours Int,
            //@OSRemindMeCount Int,
            //@CurrentDate Date = getdate(),
            //@RemainingDays nVarchar(10)

            //-- Variable value assignment
            //Select @MessageGracefulAppVersion = LegalDocument from uatb_DocumentLibrary Nolock Where Title = 'AppReleaseUpdates_MessageGracefulAppVersion'
            //Select @MessageForceUpdateAppVersion = LegalDocument from uatb_DocumentLibrary Nolock Where Title = 'AppReleaseUpdates_MessageForceUpdateAppVersion'
            //Select @MessagePostGraceAppVersion = LegalDocument from uatb_DocumentLibrary Nolock Where Title = 'AppReleaseUpdates_MessagePostGraceAppVersion'
            //Select @MessageOSVersion = LegalDocument from uatb_DocumentLibrary Nolock Where Title = 'AppReleaseUpdates_MsgOSVersion'
            //Select @StrRemindMe = LegalDocument from uatb_DocumentLibrary Nolock Where Title = 'AppReleaseUpdates_CTARemindMe'
            //Select @StrUpdateNow = LegalDocument from uatb_DocumentLibrary Nolock Where Title = 'AppReleaseUpdates_CTAUpdateNow'
            //--Select @StrOK = LegalDocument from uatb_DocumentLibrary Nolock Where Title = 'AppReleaseUpdates_CTAOK'

            //-- Core Logic
            //------------------


            //If((select count(*) from uatb_AppReleaseUpdates Nolock Where ApplicationID = @ApplicationID and Active = 1) != 0)
            //                Begin
            //                -- Fetch and assigned to the variable for the most recent active entry from the uatb_AppReleaseUpdates table
            //                Select top 1 @SupportOSVersion = OSVersion, @SupportAppVersion = AppVersion, @StartDate = NewAppVersion_StartDate, @EndDate = OldAppVersionSupported_EndDate,
            //                @GracefulAppRemindMeInterval_Hours = RemindMeInterval_Hours, @GracefulAppRemindMeCount = RemindMeCount, @PostGraceAppRemindMeInterval_Hours = GracePeriod_RemindMeInterval_Hours,
            //                @PostGraceAppRemindMeCount = GracePeriod_RemindMeCount, @OSRemindMeInterval_Hours = OSSupport_RemindMeInterval_Hours, @OSRemindMeCount = OSSupport_RemindMeCount, @LogRemindMe_UpdateNow_Message = LogRemindMe_UpdateNow_Message
            //                from uatb_AppReleaseUpdates Nolock Where ApplicationID = @ApplicationID and Active = 1 Order by Id Desc


            //                IF(dbo.uafunc_IsOSVersionGreaterorEqual(@OSVersion, @SupportOSVersion) = 0)
            //                Begin
            //                If(@OSRemindMeCounter <= @OSRemindMeCount)
            //                Begin
            //                --Set @DisplayMessage = Replace(@MessageOSVersion, '%d', @ApplicationName)
            //                Set @DisplayMessage = Replace(@MessageOSVersion, '%d', Case When @ApplicationID = 1 Then 'iOS' When @ApplicationID = 2 Then 'android' End)
            //                Set @CTARemindMe = @StrRemindMe
            //                Set @CTAUpdateNow = @StrUpdateNow
            //                Set @RemindMeInterval_Hours = @OSRemindMeInterval_Hours
            //                Set @RemindMeInterval_DateTime = DATEADD(HH, @OSRemindMeInterval_Hours, GETUTCDATE())
            //                End
            //                Else
            //                Begin
            //                Set @DisplayMessage = Replace(@MessageOSVersion, '%d', Case When @ApplicationID = 1 Then 'iOS' When @ApplicationID = 2 Then 'android' End)
            //                Set @CTAUpdateNow = @StrUpdateNow
            //                Set @RemindMeInterval_Hours = 0
            //                Set @RemindMeInterval_DateTime = ''
            //                End
            //                End
            //                Else
            //                Begin
            //                If(dbo.uafunc_IsApplicationVersionGreaterorEqual(@AppVersion, @SupportAppVersion) = 0)
            //                Begin
            //                If DateDiff(day, @CurrentDate, @EndDate) > 0-- Today < EndDate
            //                Begin
            //                If @AppRemindMeCounter <= @GracefulAppRemindMeCount
            //                Begin
            //                Set @RemainingDays = DateDiff(day, @CurrentDate, @EndDate)


            //                Set @RemainingDays = Case When @RemainingDays = '1' Then 'one day' When @RemainingDays = '2' Then 'two days' When @RemainingDays = '3' Then 'three days'
            //                When @RemainingDays = '4' Then 'four days' When @RemainingDays = '5' Then 'five days' When @RemainingDays = '6' Then 'six days'
            //                When @RemainingDays = '7' Then 'seven days' When @RemainingDays = '8' Then 'eight days' When @RemainingDays = '9' Then 'nine days'
            //                Else @RemainingDays + ' days' End
            //                Set @DisplayMessage = Replace(@MessageGracefulAppVersion, '%d', @RemainingDays)
            //                Set @CTARemindMe = @StrRemindMe
            //                Set @CTAUpdateNow = @StrUpdateNow
            //                Set @RemindMeInterval_Hours = @GracefulAppRemindMeInterval_Hours
            //                Set @RemindMeInterval_DateTime = DATEADD(HH, @GracefulAppRemindMeInterval_Hours, GETUTCDATE())
            //                End
            //                End
            //                Else If DateDiff(day, @CurrentDate, @EndDate) = 0-- Today = EndDate
            //                Begin
            //                If @AppRemindMeCounter <= @PostGraceAppRemindMeCount
            //                Begin
            //                Set @DisplayMessage = @MessagePostGraceAppVersion
            //                Set @CTARemindMe = @StrRemindMe
            //                Set @CTAUpdateNow = @StrUpdateNow
            //                Set @RemindMeInterval_Hours = @PostGraceAppRemindMeInterval_Hours
            //                Set @RemindMeInterval_DateTime = DATEADD(HH, @PostGraceAppRemindMeInterval_Hours, GETUTCDATE())
            //                End
            //                Else
            //                Begin
            //                Set @DisplayMessage = @MessageForceUpdateAppVersion
            //                Set @CTAUpdateNow = @StrUpdateNow
            //                Set @RemindMeInterval_Hours = 0
            //                Set @RemindMeInterval_DateTime = ''
            //                End
            //                End
            //                Else If DateDiff(day, @CurrentDate, @EndDate) < 0-- Today > EndDate
            //                Begin
            //                If @AppRemindMeCounter <= @PostGraceAppRemindMeCount
            //                Begin
            //                Set @DisplayMessage = @MessagePostGraceAppVersion
            //                Set @CTARemindMe = @StrRemindMe
            //                Set @CTAUpdateNow = @StrUpdateNow
            //                Set @RemindMeInterval_Hours = @PostGraceAppRemindMeInterval_Hours
            //                Set @RemindMeInterval_DateTime = DATEADD(HH, @PostGraceAppRemindMeInterval_Hours, GETUTCDATE())
            //                End
            //                Else
            //                Begin
            //                Set @DisplayMessage = @MessageForceUpdateAppVersion
            //                Set @CTAUpdateNow = @StrUpdateNow
            //                Set @RemindMeInterval_Hours = 0
            //                Set @RemindMeInterval_DateTime = ''
            //                End
            //                End
            //                End
            //                End
            //                End


            //                Select @DisplayMessage as 'DisplayMessage', @CTARemindMe as 'CTARemindMe', @CTAUpdateNow as 'CTAUpdateNow', @CTAOK as 'CTAOK', @RemindMeInterval_Hours as 'RemindMeInterval_Hours', @RemindMeInterval_DateTime as 'RemindMeInterval_DateTime',
            //                @LogRemindMe_UpdateNow_Message as 'LogRemindMe_UpdateNow_Message'


            //                END
            #endregion
            try
            {
                string key = osVersion + "::" + appRemindMeCounter;
                return await _dynamoDBService.GetRecords<T>(_configuration?.GetValue<string>("DynamoDBTables:uatb_AppReleaseUpdates"), "releaseUpdate01", key, sessionId);
            }
            catch
            {

            }

            return default;
        }
    }
}
