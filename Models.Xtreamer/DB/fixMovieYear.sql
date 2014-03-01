-- ----------------------------
-- Table structure for "EdmMetadata"
-- ----------------------------
CREATE TABLE IF NOT EXISTS "EdmMetadata" (
	"Id"  TEXT,
	"ModelHash"  TEXT
);

-- ----------------------------
-- Table structure for "__MigrationHistory"
-- ----------------------------
CREATE TABLE IF NOT EXISTS "__MigrationHistory" (
	"MigrationId"  TEXT NOT NULL,
	"Model"  BLOB NOT NULL,
	"ProductVersion"  TEXT NOT NULL
);


UPDATE movies
SET year=NULL
WHERE id NOT IN(SELECT id FROM movies WHERE year >= 0 AND year < 9999 OR year IS NULL);