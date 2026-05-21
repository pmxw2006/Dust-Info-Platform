import unittest
from selenium import webdriver
from DengLuCheSHI.zhucijmian import zhucifangfa
from selenium.webdriver.chrome.options import Options
from DengLuCheSHI.DengLuFongZhuang import Loginpage
class wjmm_tet(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        chrome_options = Options()
        chrome_options.add_argument("--user-agent=MyTestRunner/1.0")
        cls.driver = webdriver.Chrome(options=chrome_options)
        cls.driver.maximize_window()
        cls.gz = zhucifangfa(cls.driver)
    @classmethod
    def tearDownClass(cls):
        cls.driver.quit()
    def setUp(self):
        self.driver = self.__class__.driver  # 重新绑定，覆盖掉之前可能的错误赋值
        self.driver.delete_all_cookies()
        self.gz.a1("https://aloft-rectify-salvaging.ngrok-free.dev/DengLu/TianJiaZhangHu")
    def test_cs1(self):
        #输入正确的用户名，邮箱，密码，和确定密码
        self.gz.a2("天下无双")
        self.gz.a3("1452547270@qq.com")
        self.gz.a4("123456")
        self.gz.a5("123456")
        self.gz.a6()
        self.assertTrue(self.gz.a13(),"断言失败未能跳转到后台")
    def test_cs2(self):
        #输入长度为一的用户名，邮箱，密码，和确定密码
        self.gz.a2("天")
        self.gz.a3("1452547270@qq.com")
        self.gz.a4("123456")
        self.gz.a5("123456")
        self.gz.a6()
        self.assertIn("用户名长度需在3-10位之间", self.gz.a8())
    def test_cs3(self):
        #输入长度超过限制的用户名，邮箱，密码，和确定密码
        self.gz.a2("天" * 221)
        self.gz.a3("1452547270@qq.com")
        self.gz.a4("123456")
        self.gz.a5("123456")
        self.gz.a6()
        self.assertIn("用户名长度需在3-10位之间", self.gz.a8())
    def test_cs4(self):
        #输入长度为空的用户名，邮箱，密码，和确定密码
        self.gz.a2("")
        self.gz.a3("1452547270@qq.com")
        self.gz.a4("123456")
        self.gz.a5("123456")
        self.gz.a6()
        self.assertIn("用户名不能为空", self.gz.a8())
    def test_cs5(self):
        #输入正确格式带特殊字符用户名，邮箱，密码，和确定密码
        self.gz.a2("12#￥%")
        self.gz.a3("1452547270@qq.com")
        self.gz.a4("123456")
        self.gz.a5("123456")
        self.gz.a6()
        self.assertTrue(self.gz.a13(),"断言失败未能跳转到后台")
    def test_cs6(self):
        #输入正确用户名，不带@邮箱，正确的密码，和正确的确定密码
        self.gz.a2("天下无双")
        self.gz.a3("1452547270qq.com")
        self.gz.a4("123456")
        self.gz.a5("123456")
        self.gz.a6()
        self.assertIn("邮箱格式不正确", self.gz.a9())
    def test_cs7(self):
        #输入正确用户名，不带.邮箱，正确的密码，和正确的确定密码
        self.gz.a2("天下无双")
        self.gz.a3("1452547270@qqcom")
        self.gz.a4("123456")
        self.gz.a5("123456")
        self.gz.a6()
        self.assertIn("邮箱格式不正确", self.gz.a9())
    def test_cs8(self):
        #输入正确用户名，空的邮箱，正确的密码，和正确的确定密码
        self.gz.a2("天下无双")
        self.gz.a3("")
        self.gz.a4("123456")
        self.gz.a5("123456")
        self.gz.a6()
        self.assertIn("邮箱不能为空", self.gz.a9())
    def test_cs9(self):
        #输入正确用户名，空的邮箱，正确的密码，和正确的确定密码
        self.gz.a2("天下无双")
        self.gz.a3("")
        self.gz.a4("123456")
        self.gz.a5("123456")
        self.gz.a6()
        self.assertIn("邮箱不能为空", self.gz.a9())
    def test_cs10(self):
        #输入正确用户名，空的邮箱，正确的密码，和正确的确定密码
        self.gz.a2("天下无双")
        self.gz.a3("")
        self.gz.a4("123456")
        self.gz.a5("123456")
        self.gz.a6()
        self.assertIn("邮箱不能为空", self.gz.a9())
    def test_cs11(self):
        #输入正确的用户名，邮箱，和长度为1密码，和确定密码
        self.gz.a2("天下无双")
        self.gz.a3("1452547270@qq.com")
        self.gz.a4("1")
        self.gz.a5("1")
        self.gz.a6()
        self.assertIn("密码长度需在6-20位之间", self.gz.a10())
    def test_cs12(self):
        #输入正确的用户名，邮箱，和长度为21的密码，和确定密码
        self.gz.a2("天下无双")
        self.gz.a3("1452547270@qq.com")
        self.gz.a4("1" * 21)
        self.gz.a5("1" * 21)
        self.gz.a6()
        self.assertIn("密码长度需在6-20位之间", self.gz.a10())
    def test_cs13(self):
        #输入正确的用户名，邮箱，和长度为空的密码，和确定密码
        self.gz.a2("天下无双")
        self.gz.a3("1452547270@qq.com")
        self.gz.a4("")
        self.gz.a5("")
        self.gz.a6()
        self.assertIn("密码不能为空", self.gz.a10())
    def test_cs14(self):
        #输入正确的用户名，邮箱，和带有特殊字符的密码，和确定密码
        self.gz.a2("天下无双")
        self.gz.a3("1452547270@qq.com")
        self.gz.a4("！@#123")
        self.gz.a5("！@#123")
        self.gz.a6()
        self.assertTrue(self.gz.a13(),"断言失败未能跳转到后台")
    def test_cs15(self):
        #输入正确的用户名，邮箱，的密码，和不一样的确定密码
        self.gz.a2("天下无双")
        self.gz.a3("1452547270@qq.com")
        self.gz.a4("123456")
        self.gz.a5("123457")
        self.gz.a6()
        self.assertIn("两次输入的密码不一致", self.gz.a11())
    def test_cs16(self):
        #输入正确的用户名，邮箱，的密码，和不一样的确定密码
        self.gz.a2("天下无双")
        self.gz.a3("1452547270@qq.com")
        self.gz.a4("123456")
        self.gz.a5("")
        self.gz.a6()
        self.assertIn("请确认密码", self.gz.a11())
    def test_cs17(self):
        #输入正确的用户名，邮箱，的密码，和不一样的确定密码
        self.gz.a2("天下无双")
        self.gz.a3("1452547270@qq.com")
        self.gz.a4("123456")
        self.gz.a5("")
        self.gz.a7()
        self.assertTrue(self.gz.a14(),"断言失败未能跳转到后台")



