COMPONENTS := Server Manager Client
TOPTARGETS := all clean

.PHONY: $(TOPTARGETS) $(COMPONENTS)

TOPDIR ?= $(CURDIR)

IN_SERVER_DIR	:=	Server/
IN_MANAGER_DIR	:=	Manager/SwitchPresence-Rewritten-Manager.nro

OUT_DIR 		:= 	out
OUT_SERVER_DIR	:=	Sysmodule
OUT_MANAGER_DIR	:=	

all: $(COMPONENTS)
	rm -rf $(OUT_DIR)
#	Sysmodule files
	mkdir -p $(OUT_DIR)/$(OUT_SERVER_DIR)/atmosphere/contents/0100000000000464/flags
	touch $(OUT_DIR)/$(OUT_SERVER_DIR)/atmosphere/contents/0100000000000464/flags/boot2.flag
	cp $(IN_SERVER_DIR)SwitchPresence-Rewritten.nsp 		$(OUT_DIR)/$(OUT_SERVER_DIR)/atmosphere/contents/0100000000000464/exefs.nsp
	cp $(IN_SERVER_DIR)toolbox.json 		$(OUT_DIR)/$(OUT_SERVER_DIR)/atmosphere/contents/0100000000000464/toolbox.json
#	Manager files
	cp $(IN_MANAGER_DIR) 		$(OUT_DIR)/$(OUT_MANAGER_DIR)/SwitchPresence-Rewritten-Manager.nro
#	Zipping up files
	@echo Zipping files now...
	cd $(TOPDIR)/$(OUT_DIR)/$(OUT_SERVER_DIR); 	zip -r $(TOPDIR)/$(OUT_DIR)/Sysmodule.zip ./;	cd $(TOPDIR);
	rm -rf $(TOPDIR)/$(OUT_DIR)/$(OUT_SERVER_DIR)
	@echo [ZIPPED] All the files have been zipped and placed in $(OUT_DIR)/.
	@echo [DONE]


Manager:
	$(MAKE) -C $@

Server:
	$(MAKE) -C $@

clean:
	$(MAKE) -C Server clean
	$(MAKE) -C Manager clean
	rm -rf $(OUT_DIR)