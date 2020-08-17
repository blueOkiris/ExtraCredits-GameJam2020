# Build Settings
OBJNAME :=   game-engine

SRCFLDRS :=  src src/engine src/res
INCFLDRS :=  include include/engine include/res

CPPC :=      g++
CPPFLAGS :=  -O2 -Wall
LDFLAGS :=   

# Autogen settings
HFILES :=    $(foreach folder,$(INCFLDRS),$(wildcard $(folder)/*.hpp))
INC :=       $(addprefix -I,$(INCFLDRS))
SRC :=       $(foreach folder,$(SRCFLDRS),$(wildcard $(folder)/*.cpp))

# Targets
$(OBJNAME) : $(SRCFILES) $(HFILES)
	mkdir -p obj
	$(foreach file,$(SRC),\
		$(CPPC) $(INC) $(CPPFLAGS) -c $(file) -o obj/$(basename $(notdir $(file))).o; \
	)
	$(CPPC) -o $(OBJNAME) $(foreach file,$(SRC),obj/$(basename $(notdir $(file))).o) $(LDFLAGS)


.PHONY : clean
clean :
	rm -rf obj
	rm -rf $(OBJNAME)
