USE dbperformance1
GO
ALTER PROCEDURE GetReportsByEngineers
	@engName varchar(40)
AS
BEGIN
	DECLARE @eng_id int;

	SELECT @eng_id = e.Id 
	FROM dbo.Engineers AS e
	WHERE e.Name = @engName;

	SELECT r.Id, 
		r.StartDate, 
		r.EndDate, 
		r.Title, 
		t.Name AS TopicName, 
		t.Id AS TopicId,
		p.Id AS PartnerId,
		p.Name AS PartnerName,
		tf.Name AS TopicFamily,
		EngList=STUFF('[ ]',2,1,(SELECT '{Name: '+ Name + ', Email : ' + Email + '}, ' FROM dbo.Engineers AS ein WHERE ein.Id in (
			SELECT DISTINCT rein.Engineer_Id FROM dbo.ReportEngineers AS rein WHERE rein.Report_Id = r.id)
			FOR XML PATH('')))
		FROM dbo.Reports AS r
		JOIN dbo.ReportEngineers AS re ON r.Id = re.Report_Id
		JOIN dbo.Topics AS t ON r.Topic_Id = t.Id
		JOIN dbo.Partners AS p ON r.Partner_Id = p.Id
		JOIN dbo.TopicFamilies AS tf on tf.Id = t.TopicFamilyId
		WHERE re.Engineer_Id = @eng_id
END