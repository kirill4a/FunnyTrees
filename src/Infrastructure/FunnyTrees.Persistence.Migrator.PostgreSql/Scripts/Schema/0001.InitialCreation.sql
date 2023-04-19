/* TreeNode table */
CREATE SEQUENCE public.treenode_id_seq
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 9223372036854775807
    CACHE 1;

ALTER SEQUENCE public.treenode_id_seq
    OWNER TO postgres;
	
CREATE TABLE IF NOT EXISTS public."TreeNode"
(
    "Id" integer NOT NULL DEFAULT nextval('treenode_id_seq'::regclass),
    "ParentId" integer NULL,
    "Name" character varying(100) NOT NULL,
    --"DeactivateAt" timestamp with time zone NOT NULL,
    CONSTRAINT "TreeNode_pkey" PRIMARY KEY ("Id"),
	
    CONSTRAINT "FK_TreeNode_Parent" FOREIGN KEY ("ParentId")
        REFERENCES public."TreeNode" ("Id") MATCH SIMPLE
);

CREATE UNIQUE INDEX "IX_Unique_NotNull_TreeNode_Name_ParentId" ON  public."TreeNode" ("Name", "ParentId")
WHERE "ParentId" IS NOT NULL;

CREATE UNIQUE INDEX "IX_Unique_Null_TreeNode_Name_ParentId" ON  public."TreeNode" ("Name", "ParentId")
WHERE "ParentId" IS NULL;

ALTER TABLE public."TreeNode"
    OWNER to postgres;

GRANT ALL ON TABLE public."TreeNode" TO postgres;

GRANT ALL ON TABLE public."TreeNode" TO postgres;


/* LogEntry table */
CREATE SEQUENCE public.logentry_id_seq
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 9223372036854775807
    CACHE 1;

ALTER SEQUENCE public.logentry_id_seq
    OWNER TO postgres;
	
CREATE TABLE IF NOT EXISTS public."LogEntry"
(
    "Id" integer NOT NULL DEFAULT nextval('logentry_id_seq'::regclass),
    "EventId" uuid NOT NULL,
    "Timestamp" timestamp with time zone NOT NULL,
    "QueryParams" character varying(512) NULL,
    "BodyParams" character varying(5000) NULL,
    "StackTrace" character varying(5000) NOT NULL,
    "Message" character varying(512) NOT NULL,
    CONSTRAINT "LogEntry_pkey" PRIMARY KEY ("Id")
);

ALTER TABLE public."LogEntry"
    OWNER to postgres;

GRANT ALL ON TABLE public."LogEntry" TO postgres;

GRANT ALL ON TABLE public."LogEntry" TO postgres;