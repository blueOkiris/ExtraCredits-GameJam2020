# Build Settings
OBJNAME :=   game-engine

SRCFLDRS :=  src src/engine src/res
INCFLDRS :=  include include/engine include/res
ifdef LINUX
    LIBFLDRS :=  lib/linux
else
    ifdef WIN64
	    LIBFLDRS := lib/win64
	endif
endif

CPPC :=      g++
CPPFLAGS :=  -O2 -Wall -pthread -std=c++14
LDFLAGS :=   -lsfml-window-s \
             -lsfml-graphics-s \
			 -lsfml-main \
			 -lsfml-system-s \
			 -lsfml-audio-s

# Autogen settings
HFILES :=    $(foreach folder,$(INCFLDRS),$(wildcard $(folder)/*.hpp))
INC :=       $(addprefix -I,$(INCFLDRS))
LIB :=       $(addprefix -L,$(LIBFLDRS))
SRC :=       $(foreach folder,$(SRCFLDRS),$(wildcard $(folder)/*.cpp))

# Targets
$(OBJNAME) : $(SRCFILES) $(HFILES)
	mkdir -p obj
	$(foreach file,$(SRC),\
		$(CPPC) $(INC) $(CPPFLAGS) -c $(file) -o obj/$(basename $(notdir $(file))).o; \
	)
	$(CPPC) $(LIB) -o $(OBJNAME) $(foreach file,$(SRC),obj/$(basename $(notdir $(file))).o) $(LDFLAGS)


.PHONY : clean
clean :
	rm -rf obj
	rm -rf $(OBJNAME)
