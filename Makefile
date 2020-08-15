OBJNAME :=       game-engine
PROJNAME :=      Game\ Jam
TARGET_FRMWRK := netcoreapp3.1

SRCFLDRS :=      src src/engine src/res
SRCFILES :=      $(foreach folder,$(SRCFLDRS),$(wildcard $(folder)/*.cs))

ifdef LINUX
    RUNTIME := linux-x64
else
    ifdef WIN32
        RUNTIME := win-x86
    else
        ifdef WIN64
            RUNTIME := win-x64
        endif
    endif
endif

$(OBJNAME) : $(SRCFILES)
	dotnet publish $(PROJNAME).csproj -f $(TARGET_FRMWRK) -p:PublishSingleFile=true -r $(RUNTIME)
	cp bin/Debug/$(TARGET_FRMWRK)/$(RUNTIME)/publish/$(PROJNAME) ./$(OBJNAME)
	chmod +x $(OBJNAME)

.PHONY : clean
clean :
	rm -rf bin
	rm -rf obj
	rm -rf $(OBJNAME)
	rm -rf /var/tmp/.net

