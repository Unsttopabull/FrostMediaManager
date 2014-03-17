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
	"ContextKey" TEXT,
	"MigrationId"  TEXT NOT NULL,
	"Model"  BLOB NOT NULL,
	"ProductVersion"  TEXT NOT NULL
);