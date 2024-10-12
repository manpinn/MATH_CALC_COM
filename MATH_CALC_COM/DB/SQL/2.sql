-- Table: public.request_data

-- DROP TABLE IF EXISTS public.request_data;

CREATE TABLE IF NOT EXISTS public.request_data
(
    id integer NOT NULL,
    date_created timestamp with time zone NOT NULL,
    url text COLLATE pg_catalog."default",
    ip_address text COLLATE pg_catalog."default",
    CONSTRAINT request_data_pkey PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.request_data
    OWNER to dba;