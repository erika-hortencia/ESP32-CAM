/* Create Tables */

CREATE TABLE `tbl_registers` (
  `int_register_id` integer PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `chr_register_name` varchar(256) UNIQUE NOT NULL,
  `blob_register_image` MEDIUMBLOB,
  `int_register_type` integer NOT NULL,
  `dte_register_creation_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
);