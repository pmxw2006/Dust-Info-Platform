import unittest
from DengLuCheSHI.zhuyeicishigongju import Loginpage
from selenium import webdriver
from selenium.webdriver.chrome.options import Options
class gz(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        chrome_options = Options()
        # 设置一个非浏览器风格的 User-Agent，绕过 ngrok 警告页
        chrome_options.add_argument("--user-agent=MyTestRunner/1.0")
        cls.driver = webdriver.Chrome(options=chrome_options)
        cls.driver.maximize_window()
        cls.gz = Loginpage(cls.driver)
    @classmethod
    def tearDownClass(cls):
        cls.driver.quit()
    def setUp(self):
        self.driver = self.__class__.driver  # 重新绑定，覆盖掉之前可能的错误赋值
        self.driver.delete_all_cookies()
        self.gz.a1("https://aloft-rectify-salvaging.ngrok-free.dev/DengLu/DengLuYe")
    def test_cs1(self):
        #浏览贴子
        self.gz.a2("10000010")
        self.gz.a3("123456")
        self.gz.a5()
        self.gz.dianjitzi()
    def test_cs2(self):
        #浏览贴子点赞
        self.gz.a2("10000010")
        self.gz.a3("123456")
        self.gz.a5()
        self.gz.dianjitzi()
        self.gz.dianzan()
    def test_cs3(self):
        #浏览贴子取消点赞
        self.gz.a2("10000010")
        self.gz.a3("123456")
        self.gz.a5()
        self.gz.dianjitzi()
        self.gz.dianzan()
    def test_cs4(self):
        #浏览贴子评论
        self.gz.a2("10000010")
        self.gz.a3("123456")
        self.gz.a5()
        self.gz.dianjitzi()
        self.gz.pinglun()
        self.gz.shurukuamng("太帅啦")
        self.gz.fasong()
    def test_cs5(self):
        #回复
        self.gz.a2("10000010")
        self.gz.a3("123456")
        self.gz.a5()
        self.gz.dianjitzi()
        self.gz.huifu()
        self.gz.shurukuamng("真的太帅啦")
        self.gz.fasong()
    def test_cs6(self):
        #回复
        self.gz.a2("10000010")
        self.gz.a3("123456")
        self.gz.a5()
        self.gz.dianjitzi()
        self.gz.shanchuplun()
    def test_cs7(self):
        #回复
        self.gz.a2("10000010")
        self.gz.a3("123456")
        self.gz.a5()
        self.gz.dianjitzi()
        self.gz.shanyiyei()




