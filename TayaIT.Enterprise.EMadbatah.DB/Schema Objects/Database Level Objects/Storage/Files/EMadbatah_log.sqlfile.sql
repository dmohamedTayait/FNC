﻿ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [EMadbatah_log], FILENAME = '$(Path1)$(DatabaseName)_log.ldf', MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);

