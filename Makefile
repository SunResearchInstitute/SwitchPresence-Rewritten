COMPONENTS := Server Manager Client
TOPTARGETS := all clean

.PHONY: $(TOPTARGETS) $(COMPONENTS)

TOPDIR ?= $(CURDIR)

IN_GUI_DIR		:=	Client/SwitchPresence-Rewritten-GUI/bin/Release
IN_CLI_DIR		:=	Client/SwitchPresence-CLI/bin/Release/netcoreapp3.1
IN_SERVER_DIR	:=	Server/
IN_MANAGER_DIR	:=	Manager/SwitchPresence-Rewritten-Manager.nro

OUT_DIR 		:= 	out
OUT_GUI_DIR		:=	GUI-Client
OUT_CLI_DIR		:=	CLI-Client
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
#	Client GUI files
	mkdir -p $(OUT_DIR)/$(OUT_GUI_DIR)
	cp $(IN_GUI_DIR)/DiscordRPC.dll 					$(OUT_DIR)/$(OUT_GUI_DIR)
	cp $(IN_GUI_DIR)/Newtonsoft.Json.dll 				$(OUT_DIR)/$(OUT_GUI_DIR)
	cp $(IN_GUI_DIR)/PresenceCommon.dll 				$(OUT_DIR)/$(OUT_GUI_DIR)
	cp $(IN_GUI_DIR)/SwitchPresence-Rewritten-GUI.exe 	$(OUT_DIR)/$(OUT_GUI_DIR)
#	Client CLI files
	mkdir -p $(OUT_DIR)/$(OUT_CLI_DIR)
	cp $(IN_CLI_DIR)/CommandLine.dll 						$(OUT_DIR)/$(OUT_CLI_DIR)
	cp $(IN_CLI_DIR)/DiscordRPC.dll 						$(OUT_DIR)/$(OUT_CLI_DIR)
	cp $(IN_CLI_DIR)/Newtonsoft.Json.dll 					$(OUT_DIR)/$(OUT_CLI_DIR)
	cp $(IN_CLI_DIR)/PresenceCommon.dll 					$(OUT_DIR)/$(OUT_CLI_DIR)
	cp $(IN_CLI_DIR)/SwitchPresence-CLI.dll 				$(OUT_DIR)/$(OUT_CLI_DIR)
	cp $(IN_CLI_DIR)/SwitchPresence-CLI.runtimeconfig.json 	$(OUT_DIR)/$(OUT_CLI_DIR)
	cp $(IN_CLI_DIR)/SwitchPresence-CLI.exe 				$(OUT_DIR)/$(OUT_CLI_DIR)
	@echo [BUILT] SwitchPresence-Rewritten compiled successfully. All files have been placed in $(OUT_DIR)/
#	Zipping up files
	@echo Zipping files now...
	cd $(TOPDIR)/$(OUT_DIR)/$(OUT_CLI_DIR); 	zip -r $(TOPDIR)/$(OUT_DIR)/CLI-Client.zip ./;	cd $(TOPDIR);
	cd $(TOPDIR)/$(OUT_DIR)/$(OUT_GUI_DIR); 	zip -r $(TOPDIR)/$(OUT_DIR)/GUI-Client.zip ./;	cd $(TOPDIR);
	cd $(TOPDIR)/$(OUT_DIR)/$(OUT_SERVER_DIR); 	zip -r $(TOPDIR)/$(OUT_DIR)/Sysmodule.zip ./;	cd $(TOPDIR);
	rm -rf $(TOPDIR)/$(OUT_DIR)/$(OUT_CLI_DIR)
	rm -rf $(TOPDIR)/$(OUT_DIR)/$(OUT_GUI_DIR)
	rm -rf $(TOPDIR)/$(OUT_DIR)/$(OUT_SERVER_DIR)
	@echo [ZIPPED] All the files have been zipped and placed in $(OUT_DIR)/.
	@echo [DONE]


Client:
	msbuild.exe $@ -restore -nologo -v:m -m -t:Build -p:Configuration=Release

Manager:
	$(MAKE) -C $@

Server:
	$(MAKE) -C $@

clean:
	$(MAKE) -C Server clean
	$(MAKE) -C Manager clean
	msbuild.exe Client -nologo -v:m -m -t:Clean -p:Configuration=Release
	rm -rf $(OUT_DIR)