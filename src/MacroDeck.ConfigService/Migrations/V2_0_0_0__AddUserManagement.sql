CREATE SEQUENCE config_service.user_id_seq START WITH 1 INCREMENT BY 1;
ALTER SEQUENCE config_service.user_id_seq OWNER TO configservicemaster;
CREATE TABLE config_service.user
(
    u_id                   integer DEFAULT nextval('config_service.user_id_seq') PRIMARY KEY,
    u_user_name            text NOT NULL unique,
    u_password_hash        text NOT NULL,
    u_password_salt        text NOT NULL,
    u_last_login           timestamp NOT NULL,
    u_role                 integer NOT NULL,
    u_created_timestamp    timestamp NOT NULL,
    u_updated_timestamp    timestamp
);
ALTER TABLE config_service.user OWNER TO configservicemaster;

CREATE INDEX "u_user_name_idx" on config_service.user (u_user_name);