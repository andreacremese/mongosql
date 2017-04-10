USE dbperformance1
GO
CREATE FUNCTION GetReportsByEngineersFunc
	(
		@engName VARCHAR(40)
	)
	RETURNS 
	@reports TABLE (
		[Id] INT NOT NULL PRIMARY KEY,
		[StartDate] DATETIME,
		[EndDate] DATETIME,
		[Title] VARCHAR(100)
	)
	AS
	BEGIN
		DECLARE @eng_id int;

		SELECT @eng_id = e.Id 
		FROM dbo.Engineers AS e
		WHERE e.Name = @engName

		INSERT INTO @reports
		SELECT r.Id, r.StartDate, r.EndDate, r.Title
		FROM dbo.Reports AS r
		JOIN dbo.ReportEngineers AS re ON r.Id = re.Report_Id
		WHERE re.Engineer_Id = @eng_id 

		RETURN

	END