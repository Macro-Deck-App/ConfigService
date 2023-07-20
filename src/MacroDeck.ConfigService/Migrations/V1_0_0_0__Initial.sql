CREATE SCHEMA IF NOT EXISTS config_service;

CREATE SEQUENCE config_service.config_id_seq START WITH 1 INCREMENT BY 1;
ALTER SEQUENCE config_service.config_id_seq OWNER TO configservicemaster;
CREATE TABLE config_service.config
(
    cfg_id                   integer DEFAULT nextval('config_service.config_id_seq') PRIMARY KEY,
    cfg_name                 text NOT NULL unique,
    cfg_value                text NOT NULL,
    cfg_access_token         text NOT NULL,
    cfg_access_token_salt    text NOT NULL,
    cfg_version              integer DEFAULT 1 NOT NULL,
    cfg_created_timestamp    timestamp NOT NULL,
    cfg_updated_timestamp    timestamp
);
ALTER TABLE config_service.config OWNER TO configservicemaster;

CREATE INDEX "configs_cfg_name_idx" on config_service.config (cfg_name);