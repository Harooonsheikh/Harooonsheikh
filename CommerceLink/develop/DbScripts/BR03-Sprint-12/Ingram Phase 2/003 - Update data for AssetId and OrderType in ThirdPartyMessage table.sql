SELECT	Content,
		AssetId,
		'AS' + SUBSTRING(Content, 30, 15) AssetIdToUpdate,
		OrderType,
		SUBSTRING(SUBSTRING(Content, 90, 10), 1, CHARINDEX('"', SUBSTRING(Content, 90, 10), 1) - 1) OrderTypeToUpdate
FROM	ThirdPartyMessage
WHERE	ISNULL(AssetId, '') = ''
OR		ISNULL(OrderType, '') = '';

UPDATE	ThirdPartyMessage
SET		AssetId = 'AS' + SUBSTRING(Content, 30, 15),
		OrderType =	CASE
						WHEN SUBSTRING(Content, 73, 9) = 'inquiring' THEN
							SUBSTRING(SUBSTRING(Content, 92, 10), 1, CHARINDEX('"', SUBSTRING(Content, 92, 10), 1) - 1)
						WHEN SUBSTRING(Content, 73, 7) = 'pending' THEN
							SUBSTRING(SUBSTRING(Content, 90, 10), 1, CHARINDEX('"', SUBSTRING(Content, 90, 10), 1) - 1)
					END
WHERE	ISNULL(AssetId, '') = ''
OR		ISNULL(OrderType, '') = '';

SELECT	Content,
		AssetId,
		OrderType
FROM	ThirdPartyMessage;