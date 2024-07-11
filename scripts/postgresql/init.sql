-- 请将创建database、schema以及table分开执行，postgresql不支持选择数据及模块
drop database if EXISTS shine;

create database shine with encoding 'UTF8' template=template0;

drop schema if EXISTS shine_monitoring cascade;
-- 创建shine_monitoring schema
create schema shine_monitoring;

--#region Span
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
    span_kind INT NOT NULL  CHECK (span_kind >= 0),
    trace_flags INT NOT NULL CHECK (trace_flags >= 0),
    trace_state VARCHAR(1024),
    creation_time TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    last_modification_time TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    deletion_time TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
    );

CREATE INDEX idx_shine_monitoring_span_span_id on span (span_id);
CREATE INDEX idx_shine_monitoring_span_trace_id on span (trace_id);
--#endregion

--#region SpanAttribute
CREATE TABLE IF NOT EXISTS span_attribute
(
    id VARCHAR(50) NOT NULL PRIMARY KEY,
    trace_id VARCHAR(255) NOT NULL,
    span_id VARCHAR(255) NOT NULL,
    `key` VARCHAR(255) NOT NULL,
    value_type INT NOT NULL  CHECK (value_type >= 0),
    `value` VARCHAR(255) NOT NULL,
    );

CREATE INDEX idx_shine_monitoring_span_attribute_span_id on span_attribute (span_id);
CREATE INDEX idx_shine_monitoring_span_attribute_trace_id on span_attribute (trace_id);
--#endregion

--#region ResourceAttribute
CREATE TABLE IF NOT EXISTS resource_attribute
(
    id VARCHAR(50) NOT NULL PRIMARY KEY,
    trace_id VARCHAR(255) NOT NULL,
    span_id VARCHAR(255) NOT NULL,
    `key` VARCHAR(255) NOT NULL,
    value_type INT NOT NULL  CHECK (value_type >= 0),
    `value` VARCHAR(255) NOT NULL,
    );

CREATE INDEX idx_shine_monitoring_resource_attribute_span_id on resource_attribute (span_id);
CREATE INDEX idx_shine_monitoring_resource_attribute_trace_id on resource_attribute (trace_id);
--#endregion

--#region SpanEvent
CREATE TABLE IF NOT EXISTS span_event
(
    id VARCHAR(50) NOT NULL PRIMARY KEY,
    trace_id VARCHAR(255) NOT NULL,
    span_id VARCHAR(255) NOT NULL,
    `index` int NOT NULL CHECK (`index` >= 0),
    `name` VARCHAR(255) NOT NULL,
    timestamp_unix_nano BIGINT NOT NULL CHECK (timestamp_unix_nano >= 0),
    );

CREATE INDEX idx_shine_monitoring_span_event_span_id on span_event (span_id);
CREATE INDEX idx_shine_monitoring_span_event_trace_id on span_event (trace_id);
--#endregion

--#region SpanEventAttribute
CREATE TABLE IF NOT EXISTS span_event_attribute
(
    id VARCHAR(50) NOT NULL PRIMARY KEY,
    trace_id VARCHAR(255) NOT NULL,
    span_event_index int NOT NULL CHECK (span_event_index >= 0),
    span_id VARCHAR(255) NOT NULL,
    `key` VARCHAR(255) NOT NULL,
    value_type INT NOT NULL  CHECK (value_type >= 0),
    `value` VARCHAR(255) NOT NULL,
    );

CREATE INDEX idx_shine_monitoring_span_event_attribute_span_id on span_event_attribute (span_id);
CREATE INDEX idx_shine_monitoring_span_event_attribute_trace_id on span_event_attribute (trace_id);
--#endregion

--#region SpanLink
CREATE TABLE IF NOT EXISTS span_link
(
    id VARCHAR(50) NOT NULL PRIMARY KEY,
    trace_id VARCHAR(255) NOT NULL,
    span_id VARCHAR(255) NOT NULL,
    `index` int NOT NULL CHECK (`index` >= 0),
    linked_trace_id VARCHAR(255) NOT NULL,
    linked_span_id VARCHAR(255) NOT NULL,
    linked_trace_state VARCHAR(1024),
    linked_trace_flags INT CHECK (linked_trace_flags >= 0),
    );

CREATE INDEX idx_shine_monitoring_span_link_span_id on span_link (span_id);
CREATE INDEX idx_shine_monitoring_span_link_trace_id on span_link (trace_id);
--#endregion

--#region SpanLinkAttribute
CREATE TABLE IF NOT EXISTS span_link_attribute
(
    id VARCHAR(50) NOT NULL PRIMARY KEY,
    trace_id VARCHAR(255) NOT NULL,
    span_link_index int NOT NULL CHECK (span_link_index >= 0),
    span_id VARCHAR(255) NOT NULL,
    `key` VARCHAR(255) NOT NULL,
    value_type INT NOT NULL  CHECK (value_type >= 0),
    `value` VARCHAR(255) NOT NULL,
    );

CREATE INDEX idx_shine_monitoring_span_link_attribute_span_id on span_link_attribute (span_id);
CREATE INDEX idx_shine_monitoring_span_link_attribute_trace_id on span_link_attribute (trace_id);