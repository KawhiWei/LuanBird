-- 请将创建database、schema以及table分开执行，postgresql不支持选择数据及模块
drop database if EXISTS shine;

create database shine with encoding 'UTF8' template=template0;

drop schema if EXISTS shine_monitoring cascade;
-- 创建shine_monitoring schema
create schema shine_monitoring;

--#region 环境基础数据
CREATE TABLE IF NOT EXISTS span
(
    id VARCHAR(50) NOT NULL PRIMARY KEY,
    trace_id VARCHAR(255) NOT NULL,
    span_id VARCHAR(255) NOT NULL,
    span_name VARCHAR(255) NOT NULL,
    parent_span_id VARCHAR(255),
    start_time_unix_nano BIGINT NOT NULL CHECK (start_time_unix_nano >= 0),
    end_time_unix_nano BIGINT NOT NULL CHECK (end_time_unix_nano >= 0) ,
    duration_nanoseconds BIGINT NOT NULL CHECK (duration_nanoseconds >= 0),
    service_name VARCHAR(255) NOT NULL,
    service_instance_id VARCHAR(255) NOT NULL,
    status_message VARCHAR(1024),
    status_code INT,
    span_kind INT NOT NULL  CHECK (duration_nanoseconds >= 0),
    trace_flags INT NOT NULL CHECK (trace_flags >= 0),
    trace_state VARCHAR(1024),
    );

CREATE INDEX idx_shine_monitoring_span_id on span (span_id);
CREATE INDEX idx_shine_monitoring_trace_id on span (trace_id);
--#endregion