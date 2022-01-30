using AutoMapper;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.Utilities;
using SoloDevApp.Service.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface IBuoiHocService : IService<BuoiHoc, BuoiHocViewModel>
    {
        Task<ResponseEntity> ThemListBuoiHocTheoMaLop(InputThemListBuoiHocTheoMaLopViewModel modelVm);

        Task<ResponseEntity> GetListClassesByClassId(int classId);

        Task<ResponseEntity> AddClassesToClass(int classId, int classesId);
    }

    public class BuoiHocService : ServiceBase<BuoiHoc, BuoiHocViewModel>, IBuoiHocService
    {
        private IBuoiHocRepository _buoiHocRepository;
        private ILopHocRepository _lopHocRepository;
        private IRoadMapDetailRepository _roadMapDetailRepository;
        private IBuoiHoc_NguoiDungRepository _buoiHocNguoiDungRepository;
        private IThongBaoRepository _thongBaoRepository;
        private ITrackingNguoiDungRepository _trackingNguoiDungRepository;
        private IVideoExtraRepository _videoExtraRepository;
        private ITaiLieuBaiHocRepository _taiLieuBaiHocRepository;
        private ITaiLieuBaiTapRepository _taiLieuBaiTapRepository;
        private ITaiLieuDocThemRepository _taiLieuDocThemRepository;
        private ITaiLieuProjectLamThemRepository _taiLieuProjectLamThemRepository;
        private ITaiLieuCapstoneRepository _taiLieuCapstoneRepository;
        private ITracNghiemRepository _tracNghiemRepository;
        private ITracNghiemExtraRepository _tracNghiemExtraRepository;
        private IBaiHoc_TaiLieu_Link_TracNghiemRepository _baiHoc_TaiLieu_Link_TracNghiemRepository;
        private INguoiDungRepository _nguoiDungRepository;
        private IBaiHocRepository _baiHocRepository;
        private IChuongHocRepository _chuongHocRepository;
        private IKhoaHocRepository _khoaHocRepository;
        //Tạm thời
        private IXemLaiBuoiHocRepository _xemLaiBuoiHocRepository;

        public BuoiHocService(IBuoiHocRepository buoiHocRepository, ILopHocRepository lopHocRepository, IRoadMapDetailRepository roadMapDetailRepository, IBuoiHoc_NguoiDungRepository buoiHoc_NguoiDungRepository,
            IThongBaoRepository thongBaoRepository,
            ITrackingNguoiDungRepository trackingNguoiDungRepository, IVideoExtraRepository videoExtraRepository,
            ITaiLieuBaiHocRepository taiLieuBaiHocRepository,
            ITaiLieuBaiTapRepository taiLieuBaiTapRepository,
            ITaiLieuDocThemRepository taiLieuDocThemRepository,
            ITaiLieuProjectLamThemRepository taiLieuProjectLamThemRepository,
            ITaiLieuCapstoneRepository taiLieuCapstoneRepository,
            ITracNghiemRepository tracNghiemRepository,
            ITracNghiemExtraRepository tracNghiemExtraRepository,
            IBaiHoc_TaiLieu_Link_TracNghiemRepository baiHoc_TaiLieu_Link_TracNghiemRepository,
            IXemLaiBuoiHocRepository xemLaiBuoiHocRepository,
            INguoiDungRepository nguoiDungRepository,
             IBaiHocRepository baiHocRepository,
              IChuongHocRepository chuongHocRepository,
            IKhoaHocRepository khoaHocRepository,
            IMapper mapper)
            : base(buoiHocRepository, mapper)
        {
            _buoiHocRepository = buoiHocRepository;
            _lopHocRepository = lopHocRepository;
            _buoiHocRepository = buoiHocRepository;
            _baiHoc_TaiLieu_Link_TracNghiemRepository = baiHoc_TaiLieu_Link_TracNghiemRepository;
            _videoExtraRepository = videoExtraRepository;
            _taiLieuBaiHocRepository = taiLieuBaiHocRepository;
            _taiLieuBaiTapRepository = taiLieuBaiTapRepository;
            _taiLieuDocThemRepository = taiLieuDocThemRepository;
            _taiLieuProjectLamThemRepository = taiLieuProjectLamThemRepository;
            _taiLieuCapstoneRepository = taiLieuCapstoneRepository;
            _tracNghiemRepository = tracNghiemRepository;
            _tracNghiemExtraRepository = tracNghiemExtraRepository;
            _buoiHocNguoiDungRepository = buoiHoc_NguoiDungRepository;
            _thongBaoRepository = thongBaoRepository;
            _trackingNguoiDungRepository = trackingNguoiDungRepository;
            _roadMapDetailRepository = roadMapDetailRepository;
            _xemLaiBuoiHocRepository = xemLaiBuoiHocRepository;
            _chuongHocRepository = chuongHocRepository;
            _baiHocRepository = baiHocRepository;
            _khoaHocRepository = khoaHocRepository;
            _nguoiDungRepository = nguoiDungRepository;
        }

        public async override Task<ResponseEntity> InsertAsync(BuoiHocViewModel modelVm)
        {
            try
            {

                //Kiểm tra lớp học có trong hệ thống hay không
                LopHoc lopHoc = await _lopHocRepository.GetSingleByIdAsync(modelVm.MaLop);
                if (lopHoc == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);
                }

                //Kiểm tra road map detail có trong hệ thống hay không
                RoadMapDetail roadMapDetail = await _roadMapDetailRepository.GetSingleByIdAsync(modelVm.MaRoadMapDetail);
                if (roadMapDetail == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);
                }

                //Thêm buổi học vào hệ thống
                BuoiHoc buoiHoc = _mapper.Map<BuoiHoc>(modelVm);

                //Kiểm tra nếu không truyền BiDanh lên thì tạo BiDanh hoặc convert BiDanh về đúng dạng
                if (buoiHoc.BiDanh == null || buoiHoc.BiDanh.Trim().Length == 0)
                {
                    buoiHoc.BiDanh = FuncUtilities.BestLower(buoiHoc.TenBuoiHoc);
                }
                buoiHoc.BiDanh = FuncUtilities.BestLower(buoiHoc.BiDanh);

                buoiHoc = await _buoiHocRepository.InsertAsync(buoiHoc);

                //Thêm thất bại
                if (buoiHoc == null)
                {
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.INSERT_ERROR);
                }

                //Lấy ra danh sách các buổi học của lớp
                LopHoc lopHocHienTai = await _lopHocRepository.GetSingleByIdAsync(buoiHoc.MaLop);

                List<int> lsCacBuoiHoc = JsonConvert.DeserializeObject<List<int>>(lopHocHienTai.DanhSachBuoi);

                lsCacBuoiHoc.Add(buoiHoc.Id);

                String lsCacBuoiHocString = JsonConvert.SerializeObject(lsCacBuoiHoc);

                lopHocHienTai.DanhSachBuoi = lsCacBuoiHocString;

                if ((await _lopHocRepository.UpdateAsync(buoiHoc.MaLop, lopHocHienTai)) == null)
                {
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, buoiHoc, MessageConstants.INSERT_ERROR);
                }


                return new ResponseEntity(StatusCodeConstants.OK, buoiHoc, MessageConstants.INSERT_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> ThemListBuoiHocTheoMaLop(InputThemListBuoiHocTheoMaLopViewModel modelVm)
        {
            try
            {
                //Kiểm tra lớp học có trong hệ thống hay không
                LopHoc lopHoc = await _lopHocRepository.GetSingleByIdAsync(modelVm.MaLop);
                if (lopHoc == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "Lớp học không tồn tại");
                }

                //Kiểm tra road map detail có trong hệ thống hay không

                if (modelVm.MaRoadMapDetail != 0)
                {
                    RoadMapDetail roadMapDetail = await _roadMapDetailRepository.GetSingleByIdAsync(modelVm.MaRoadMapDetail);
                    if (roadMapDetail == null)
                    {
                        return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "Road map không tồn tại");
                    }
                }

                int bienDemThuTuBuoiHoc = 0;
                DateTime _ngayHoc = lopHoc.NgayBatDau;



                List<int> lsMaBuoiHocNew = new List<int>();

                List<int> thoiKhoaBieu = JsonConvert.DeserializeObject<List<int>>(lopHoc.ThoiKhoaBieu);

                //Map list thời khóa biểu về dạng phù hợp
                thoiKhoaBieu = thoiKhoaBieu.ConvertAll(item => ConvertNgayTrongTuan(item));

                do
                {
                    if (thoiKhoaBieu.Contains((int)_ngayHoc.DayOfWeek))
                    {
                        bienDemThuTuBuoiHoc++;
                        BuoiHoc buoiHoc = new BuoiHoc();
                        buoiHoc.STT = bienDemThuTuBuoiHoc;
                        buoiHoc.NgayHoc = _ngayHoc;
                        buoiHoc.MaLop = modelVm.MaLop;
                        buoiHoc.MaRoadMapDetail = modelVm.MaRoadMapDetail;

                        lsMaBuoiHocNew.Add((await _buoiHocRepository.InsertAsync(buoiHoc)).Id);
                    }
                    //Tăng ngày học lên 1 
                    _ngayHoc = _ngayHoc.AddDays(1);
                } while (bienDemThuTuBuoiHoc <= modelVm.SoBuoiHocCuaLop);

                //Lấy ra lớp học và cập nhật danh sách id buổi học vào
                LopHoc lopHocModel = await _lopHocRepository.GetSingleByIdAsync(modelVm.MaLop);

                lopHocModel.DanhSachBuoi = JsonConvert.SerializeObject(lsMaBuoiHocNew);

                await _lopHocRepository.UpdateAsync(lopHocModel.Id, lopHocModel);

                ////Thêm buổi học vào hệ thống
                //BuoiHoc buoiHoc = _mapper.Map<BuoiHoc>(modelVm);

                ////Kiểm tra nếu không truyền BiDanh lên thì tạo BiDanh hoặc convert BiDanh về đúng dạng
                //if (buoiHoc.BiDanh == null || buoiHoc.BiDanh.Trim().Length == 0)
                //{
                //    buoiHoc.BiDanh = FuncUtilities.BestLower(buoiHoc.TenBuoiHoc);
                //}
                //buoiHoc.BiDanh = FuncUtilities.BestLower(buoiHoc.BiDanh);

                //buoiHoc = await _buoiHocRepository.InsertAsync(buoiHoc);

                ////Thêm thất bại
                //if (buoiHoc == null)
                //{
                //    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.INSERT_ERROR);
                //}

                ////Lấy ra danh sách các buổi học của lớp
                //LopHoc lopHocHienTai = await _lopHocRepository.GetSingleByIdAsync(buoiHoc.MaLop);

                //List<int> lsCacBuoiHoc = JsonConvert.DeserializeObject<List<int>>(lopHocHienTai.DanhSachBuoi);

                //lsCacBuoiHoc.Add(buoiHoc.Id);

                //String lsCacBuoiHocString = JsonConvert.SerializeObject(lsCacBuoiHoc);

                //lopHocHienTai.DanhSachBuoi = lsCacBuoiHocString;

                //if ((await _lopHocRepository.UpdateAsync(buoiHoc.MaLop, lopHocHienTai)) == null)
                //{
                //    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, buoiHoc, MessageConstants.INSERT_ERROR);
                //}

                return new ResponseEntity(StatusCodeConstants.OK);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> GetListClassesByClassId(int classId)
        {
            try
            {
                //set cứng tạm sau này sẽ bóc từ token vào
                string maNguoiDung = "d9699b77-f003-42c4-b050-26607079a789";

                LopHoc lopHoc = await _lopHocRepository.GetSingleByIdAsync(classId);

                if (lopHoc == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);
                }

                ThongTinBuoiHocTheoLopViewModel thongTinBuoiHocTheoLopVm = new ThongTinBuoiHocTheoLopViewModel();


                //Lấy tất cả tracking của theo mã người dùng từ đó lấy ra thông báo về học tập và bài tập
                IEnumerable<TrackingNguoiDung> lsTrackingNguoiDung = await _trackingNguoiDungRepository.GetMultiByConditionAsync("MaNguoiDung", maNguoiDung);

                if (lsTrackingNguoiDung != null)
                {
                    thongTinBuoiHocTheoLopVm.ThongBao = TrackingNguoiDungService.getThongBaoNguoiDung(lsTrackingNguoiDung);
                }

                //Lấy thông tin của lớp học
                thongTinBuoiHocTheoLopVm.ThongTinLopHoc.TenLopHoc = lopHoc.TenLopHoc;
                thongTinBuoiHocTheoLopVm.ThongTinLopHoc.BiDanh = lopHoc.BiDanh;
                thongTinBuoiHocTheoLopVm.ThongTinLopHoc.NgayBatDau = FuncUtilities.ConvertDateToString(lopHoc.NgayBatDau);
                thongTinBuoiHocTheoLopVm.ThongTinLopHoc.NgayKetThuc = FuncUtilities.ConvertDateToString(lopHoc.NgayKetThuc);
                thongTinBuoiHocTheoLopVm.ThongTinLopHoc.SoHocVien = lopHoc.SoHocVien;
                thongTinBuoiHocTheoLopVm.ThongTinLopHoc.ThoiKhoaBieu = lopHoc?.ThoiKhoaBieu;
                thongTinBuoiHocTheoLopVm.ThongTinLopHoc.Token = lopHoc?.Token;


                //Lẩy ra toàn bộ buổi học của lớp
                List<dynamic> danhSachBuoi = JsonConvert.DeserializeObject<List<dynamic>>(lopHoc.DanhSachBuoi);

                IEnumerable<BuoiHoc> dsBuoiHoc = await _buoiHocRepository.GetMultiByIdAsync(danhSachBuoi);


                //Lây ra danh sách top 3 điểm người dùng trong lớp, nếu có id của user trong top 3 thì thôi, còn không thì add id user và cho đứng thứ 4
                thongTinBuoiHocTheoLopVm.ThongKeDiemNguoiDung.AddRange(await ThongKeNhungNguoiCaoDiemNhatLop(maNguoiDung, danhSachBuoi));



                //Group danh sách buổi học theo skill và loop
                foreach (var groupSkill in dsBuoiHoc.GroupBy(x => x.MaSkill))
                {
                    BuoiHocBySkillViewModel buoiHocBySkillVm = new BuoiHocBySkillViewModel();

                    TaiLieuBuoiHocTheoSkillViewModel taiLieuBuoiHocTheoSkillVm = new TaiLieuBuoiHocTheoSkillViewModel();

                    DiemNguoiDungTheoSkillViewModel diemNguoiDungTheoSkillVm = new DiemNguoiDungTheoSkillViewModel();

                    List<BuoiHocViewModel> lsBuoiHocVm = new List<BuoiHocViewModel>();

                    diemNguoiDungTheoSkillVm.TenSkill = groupSkill.Key;
                    taiLieuBuoiHocTheoSkillVm.TenSkill = groupSkill.Key;
                    buoiHocBySkillVm.TenSkill = groupSkill.Key;

                    //Hardcode hiện thông tin active của skill nếu là HTML-CSS, BOOTSTRAP và GIT thì active còn khác thì inactive
                    //Trường hợp sau này sẽ sử dụng 1 biến cờ để check trong từng buổi học, nếu buổi học chưa tới thì sẽ inactive
                    string[] listSkill = { "HTML-CSS", "BOOTSTRAP", "GIT" };

                    if (listSkill.Contains(groupSkill.Key))
                    {
                        buoiHocBySkillVm.isActive = true;
                    }
                    else
                    {
                        buoiHocBySkillVm.isActive = false;
                    }

                    //Xử lý lấy ra các video khóa học liên quan tới skill
                    buoiHocBySkillVm.DanhSachKhoaHocBySkill = await LayVideoKhoaHocLienQuanTheoSkill(groupSkill.Key);


                    DiemPhanTramCuaBuoiHocViewModel tongDiemTrongSkill = new DiemPhanTramCuaBuoiHocViewModel();


                    //Duyệt từng buổi học trong skill để lấy data bài học

                    foreach (var buoiHoc in groupSkill)
                    {

                        List<dynamic> dsBaiHocTrongBuoi = JsonConvert.DeserializeObject<List<dynamic>>(buoiHoc.DanhSachBaiHocTracNghiem);

                        BuoiHocViewModel buoiHocVm = _mapper.Map<BuoiHocViewModel>(buoiHoc);

                        //Lấy ra dữ liệu BuoiHoc_NguoiDung về bài tập
                        List<KeyValuePair<string, dynamic>> buoiHocNguoiDungColumns = new List<KeyValuePair<string, dynamic>>();

                        //Gán cứng mã người dùng của sĩ để test sau này sẽ lấy từ token gắn ở header
                        buoiHocNguoiDungColumns.Add(new KeyValuePair<string, dynamic>("MaNguoiDung", maNguoiDung));
                        buoiHocNguoiDungColumns.Add(new KeyValuePair<string, dynamic>("MaBuoiHoc", buoiHoc.Id));

                        BuoiHoc_NguoiDung buoiHocNguoiDung = await _buoiHocNguoiDungRepository.GetSingleByListConditionAsync(buoiHocNguoiDungColumns);

                        if (buoiHocNguoiDung != null)
                        {
                            buoiHocVm.LichSuHocTap = JsonConvert.DeserializeObject<List<LichSuHocTapViewModel>>(buoiHocNguoiDung.LichSuHocTap);




                            //Xử lý tính % điểm của người dùng trong 1 buổi, duyệt tất cả object trong đó nếu là QUIZ_PURPLE thì tính bên thằng tím

                            foreach (var item in buoiHocVm.LichSuHocTap)
                            {
                                //Xử lý lấy điểm cho phần trăm
                                if (item.Diem >= 0)
                                {
                                    switch (item.LoaiBaiTap)
                                    {
                                        case "QUIZ_PURPLE":
                                            {
                                                tongDiemTrongSkill.TongPhanTramVongTronTim.Add(item.Diem * 10);

                                            }
                                            break;
                                        case "QUIZ":
                                            {
                                                tongDiemTrongSkill.TongPhanTramQuizVongTronCam.Add(item.Diem * 10);
                                            }
                                            break;
                                        default:
                                            {
                                                //Do bài tập viết chấm theo thang 100 nên không cần phải * 10
                                                tongDiemTrongSkill.TongPhanTramBaiTapVongTronCam.Add(item.Diem);
                                            }
                                            break;
                                    }
                                }
                                //Xử lý phần đổ bài tập ra chung với điểm, do bài tập nộp mà chưa chấm cũng đổ ra nên phải ở ngoài vòng kiểm tra điểm
                                BaiTapBuoiHocViewModel baiTap = new BaiTapBuoiHocViewModel();

                                //Gán các giá trị cho bài tập
                                baiTap.Diem = item.Diem;
                                baiTap.LoaiBaiTap = item.LoaiBaiTap;
                                baiTap.HanNop = item.HanNop;
                                baiTap.Id = item.MaBaiHoc;

                                dynamic thongTinBaiTap = null;

                                switch (item.LoaiBaiTap)
                                {
                                    case "QUIZ":
                                        {
                                            thongTinBaiTap = await _tracNghiemRepository.GetSingleByIdAsync(item.MaBaiHoc);
                                            baiTap.TieuDe = thongTinBaiTap.TieuDe;
                                            baiTap.NoiDung = thongTinBaiTap?.NoiDung;
                                            baiTap.MoTa = thongTinBaiTap?.MoTa;
                                            buoiHocVm.TracNghiem.Add(baiTap);
                                        }
                                        break;
                                    case "QUIZ_WRITE":
                                        {
                                            thongTinBaiTap = await _taiLieuBaiTapRepository.GetSingleByIdAsync(item.MaBaiHoc);
                                            baiTap.TieuDe = thongTinBaiTap?.TieuDe;
                                            baiTap.NoiDung = thongTinBaiTap?.NoiDung;
                                            baiTap.MoTa = thongTinBaiTap?.MoTa;
                                            buoiHocVm.TaiLieuBaiTap.Add(baiTap);
                                        }
                                        break;
                                    case "CAPSTONE":
                                        {
                                            thongTinBaiTap = await _taiLieuCapstoneRepository.GetSingleByIdAsync(item.MaBaiHoc);
                                            baiTap.TieuDe = thongTinBaiTap?.TieuDe;
                                            baiTap.NoiDung = thongTinBaiTap?.NoiDung;
                                            baiTap.MoTa = thongTinBaiTap?.MoTa;
                                            buoiHocVm.TaiLieuCapstone.Add(baiTap);
                                        }
                                        break;
                                    case "QUIZ_PURPLE":
                                        {
                                            thongTinBaiTap = await _tracNghiemExtraRepository.GetSingleByIdAsync(item.MaBaiHoc);
                                            baiTap.TieuDe = thongTinBaiTap?.TieuDe;
                                            baiTap.NoiDung = thongTinBaiTap?.NoiDung;
                                            baiTap.MoTa = thongTinBaiTap?.MoTa;
                                            buoiHocVm.TracNghiemExtra.Add(baiTap);
                                        }
                                        break;
                                }


                                //Xử lý lấy điểm và tieuDeBaiHoc cho phần xem điểm
                                DiemBaiTapViewModel diemBaiTapVm = new DiemBaiTapViewModel();
                                diemBaiTapVm.TieuDe = (await _baiHoc_TaiLieu_Link_TracNghiemRepository.GetSingleByIdAsync(item.MaBaiHoc)).TieuDe;
                                diemBaiTapVm.DiemBaiTap = item.Diem;
                                diemBaiTapVm.LoaiBaiTap = item.LoaiBaiTap;
                                diemNguoiDungTheoSkillVm.danhSachDiem.Add(diemBaiTapVm);


                            }

                            ////TaiLieuBaiTap
                            //IEnumerable<TaiLieuBaiTap> lsTaiLieuBaiTap = await _taiLieuBaiTapRepository.GetMultiByIdAsync(dsBaiHocTrongBuoi);
                            //buoiHocVm.TaiLieuBaiTap = (_mapper.Map<List<TaiLieuBaiTapViewModel>>(lsTaiLieuBaiTap));

                            ////TaiLieuCapstone
                            //IEnumerable<TaiLieuCapstone> lsTaiLieuCapstone = await _taiLieuCapstoneRepository.GetMultiByIdAsync(dsBaiHocTrongBuoi);
                            //buoiHocVm.TaiLieuCapstone = (_mapper.Map<List<TaiLieuCapstoneViewModel>>(lsTaiLieuCapstone));

                            ////TracNghiem
                            //IEnumerable<TracNghiem> lsTracNghiem = await _tracNghiemRepository.GetMultiByIdAsync(dsBaiHocTrongBuoi);
                            //buoiHocVm.TracNghiem = (_mapper.Map<List<TracNghiemViewModel>>(lsTracNghiem));

                        }

                        //Tính phần trăm của skill 
                        int phanTramCam = 0;
                        int phanTramTim = 0;
                        if (tongDiemTrongSkill.TongPhanTramBaiTapVongTronCam.Count() > 0)
                        {
                            phanTramCam += (int)(tongDiemTrongSkill.TongPhanTramBaiTapVongTronCam.Average() * 0.7);
                        }
                        if (tongDiemTrongSkill.TongPhanTramBaiTapVongTronCam.Count() > 0)
                        {
                            phanTramCam += (int)(tongDiemTrongSkill.TongPhanTramQuizVongTronCam.Average() * 0.3);
                        }
                        if (tongDiemTrongSkill.TongPhanTramVongTronTim.Count() > 0)
                        {
                            phanTramTim += (int)tongDiemTrongSkill.TongPhanTramVongTronTim.Average();
                        }

                        buoiHocBySkillVm.DiemBuoiHoc = new { phanTramCam = phanTramCam, phanTramTim = phanTramTim };

                        //Lấy ra dữ liệu của các View và gán cho buoiHocView
                        //TaiLieuBaiHoc
                        IEnumerable<TaiLieuBaiHoc> lsTaiLieuBaiHoc = await _taiLieuBaiHocRepository.GetMultiByIdAsync(dsBaiHocTrongBuoi);
                        buoiHocVm.TaiLieuBaiHoc = _mapper.Map<List<TaiLieuBaiHocViewModel>>(lsTaiLieuBaiHoc);
                        //Thêm Tài liệu vào danh sách tài liệu bài học theo skill
                        taiLieuBuoiHocTheoSkillVm.danhSachBaiHoc.AddRange(buoiHocVm.TaiLieuBaiHoc);

                        //TaiLieuDocThem
                        IEnumerable<TaiLieuDocThem> lsTaiLieuDocThem = await _taiLieuDocThemRepository.GetMultiByIdAsync(dsBaiHocTrongBuoi);
                        buoiHocVm.TaiLieuDocThem = (_mapper.Map<List<TaiLieuDocThemViewModel>>(lsTaiLieuDocThem));

                        //TaiLieuProjectLamThem
                        IEnumerable<TaiLieuProjectLamThem> lsTaiLieuProjectLamThem = await _taiLieuProjectLamThemRepository.GetMultiByIdAsync(dsBaiHocTrongBuoi);
                        buoiHocVm.TaiLieuProjectLamThem = (_mapper.Map<List<TaiLieuProjectLamThemViewModel>>(lsTaiLieuProjectLamThem));




                        //Add VideoXemLai
                        List<KeyValuePair<string, dynamic>> colums = new List<KeyValuePair<string, dynamic>>();

                        colums.Add(new KeyValuePair<string, dynamic>("MaBuoi", buoiHoc.Id));

                        IEnumerable<XemLaiBuoiHoc> lsXemLaiBuoiHoc = await _xemLaiBuoiHocRepository.GetMultiByListConditionAndAsync(colums);

                        if (lsXemLaiBuoiHoc != null)
                        {
                            foreach (XemLaiBuoiHoc video in lsXemLaiBuoiHoc)
                            {
                                XemLaiBuoiHocViewModel xemLaiBuoiHocVm = new XemLaiBuoiHocViewModel();
                                xemLaiBuoiHocVm = _mapper.Map<XemLaiBuoiHocViewModel>(video);
                                buoiHocVm.VideoXemLai.Add(xemLaiBuoiHocVm);
                            }
                        }

                        //Add các Video extra vào
                        IEnumerable<VideoExtra> lsVideoExtra = await _videoExtraRepository.GetMultiByListConditionAndAsync(colums);

                        if (lsVideoExtra != null)
                        {
                            foreach (VideoExtra video in lsVideoExtra)
                            {
                                VideoExtraViewModel videoExtraVm = new VideoExtraViewModel();
                                videoExtraVm = _mapper.Map<VideoExtraViewModel>(video);
                                buoiHocVm.VideoExtra.Add(videoExtraVm);
                            }
                        }
                        //Add buổi học vào list buổi học
                        lsBuoiHocVm.Add(buoiHocVm);
                    }

                    buoiHocBySkillVm.DanhSachBuoiHoc = lsBuoiHocVm;
                    thongTinBuoiHocTheoLopVm.DanhSachBuoiHocTheoSkill.Add(buoiHocBySkillVm);
                    thongTinBuoiHocTheoLopVm.DanhSachTaiLieuTheoSkill.Add(taiLieuBuoiHocTheoSkillVm);
                    thongTinBuoiHocTheoLopVm.DanhSachDiemBaiTapTheoSkill.Add(diemNguoiDungTheoSkillVm);
                }

                return new ResponseEntity(StatusCodeConstants.OK, thongTinBuoiHocTheoLopVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> AddClassesToClass(int classId, int classesId)
        {

            try
            {
                //Kiểm tra buổi học có trong hệ thống hay không
                BuoiHoc buoiHoc = await _buoiHocRepository.GetSingleByIdAsync(classesId);
                if (buoiHoc == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);
                }

                //Lấy ra danh sách các buổi học của lớp
                LopHoc lopHocHienTai = await _lopHocRepository.GetSingleByIdAsync(classId);

                List<int> lsCacBuoiHoc = JsonConvert.DeserializeObject<List<int>>(lopHocHienTai.DanhSachBuoi);
                //Kiểm tra xem hiện tại có chứa buổi học muốn thêm chưa

                if (lsCacBuoiHoc.Contains(classesId))
                {
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Buổi học đã tồn tại", MessageConstants.INSERT_ERROR);
                }

                lsCacBuoiHoc.Add(classesId);

                string lsCacBuoiHocString = JsonConvert.SerializeObject(lsCacBuoiHoc);

                lopHocHienTai.DanhSachBuoi = lsCacBuoiHocString;


                if ((await _lopHocRepository.UpdateAsync(classId, lopHocHienTai)) == null)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, lopHocHienTai, MessageConstants.INSERT_ERROR);


                return new ResponseEntity(StatusCodeConstants.OK, lopHocHienTai, MessageConstants.INSERT_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }

        }

        private async Task<List<dynamic>> ThongKeNhungNguoiCaoDiemNhatLop(string maNguoiDung, List<dynamic> danhSachBuoi)
        {
            //Lấy ra danh sách các buoihoc_nguoidung từ danh sách buổi để tính điểm tổng
            IEnumerable<BuoiHoc_NguoiDung> dsBuoiHocNguoiDungTrongLop = await _buoiHocNguoiDungRepository.GetTheoDanhSachMaBuoi(danhSachBuoi);

            //Group lại theo mã người dùng sau đó dùng reducer để tính điểm 
            List<dynamic> lsDiemCuaNguoiDung = new List<dynamic>();
            List<dynamic> lsKetQuaXuLy = new List<dynamic>();

            foreach (var nguoiDung in dsBuoiHocNguoiDungTrongLop.GroupBy(x => x.MaNguoiDung))
            {
                int tongDiem = 0;
                foreach (var buoi in nguoiDung)
                {
                    List<LichSuHocTap> lichSuHocTap = JsonConvert.DeserializeObject<List<LichSuHocTap>>(buoi.LichSuHocTap);

                    tongDiem += lichSuHocTap.Aggregate(0, (acc, x) => acc + x.Diem);

                }
                lsDiemCuaNguoiDung.Add(new { maNguoiDung = nguoiDung.Key, tongDiemNguoiDung = tongDiem });
            }

            //Có list danh sách người dùng và điểm rồi thì xử lý sort và lấy ra 3 người cao điểm nhất

            //Sort điểm người dùng
            List<dynamic> lsDiemCuaNguoiDungSorted = lsDiemCuaNguoiDung.OrderByDescending(nguoiDung => nguoiDung.tongDiemNguoiDung).Take(3).ToList();

            List<dynamic> lsIdNguoiDungCanLayRa = new List<dynamic>();

            foreach (var item in lsDiemCuaNguoiDungSorted)
            {
                lsIdNguoiDungCanLayRa.Add(item.maNguoiDung);
            }

            //Thêm Ids của người dùng hiện tại vào list để trả ra cùng nếu như trong list chưa có
            if (!lsIdNguoiDungCanLayRa.Contains(maNguoiDung))
            {
                lsIdNguoiDungCanLayRa.Add(maNguoiDung);
            }

            //Duyệt theo mảng listId đã được sắp xếp để ra kết quả theo đúng thứ tự
            foreach (var (nguoiDung, index) in lsIdNguoiDungCanLayRa.Select((nguoiDung, index) => (nguoiDung, index)))
            {
                //Lấy tên người dùng 
                string tenNguoiDung = (await _nguoiDungRepository.GetSingleByIdAsync(nguoiDung)).HoTen;
                //Lấy thứ tự người dùng
                int thuTuNguoiDung = index + 1;

                //Lọc lấy ra các dữ liệu của người dùng
                IEnumerable<BuoiHoc_NguoiDung> dsBuoiHocCuaNguoiDungFiltered = dsBuoiHocNguoiDungTrongLop.Where(item => item.MaNguoiDung.Equals(nguoiDung));

                //Xử lý tính điểm tổng của người dùng trong từng buổi học

                List<dynamic> lsDiemCuaTungBuoiHoc = new List<dynamic>();

                foreach (var item in dsBuoiHocCuaNguoiDungFiltered)
                {
                    int thuTuBuoiHoc = (await _buoiHocRepository.GetSingleByIdAsync(item.MaBuoiHoc)).STT;
                    int tongDiemTrongBuoi = (JsonConvert.DeserializeObject<List<LichSuHocTap>>(item.LichSuHocTap)).Aggregate(0, (acc, x) => acc + x.Diem);

                    lsDiemCuaTungBuoiHoc.Add(new { sttBuoi = thuTuBuoiHoc, tongDiem = tongDiemTrongBuoi });
                }

                lsKetQuaXuLy.Add(new { tenNguoiDung = tenNguoiDung, thuTuNguoiDung = thuTuNguoiDung, lsDiemCuaTungBuoiHoc = lsDiemCuaTungBuoiHoc });
            }

            return lsKetQuaXuLy;
        }

        private async Task<List<dynamic>> LayVideoKhoaHocLienQuanTheoSkill(string maSkill)
        {
            //Lấy ra các khóa học video liên quan của skill
            List<KeyValuePair<string, dynamic>> khoaHoccolums = new List<KeyValuePair<string, dynamic>>();

            khoaHoccolums.Add(new KeyValuePair<string, dynamic>("MaSkill", maSkill));

            IEnumerable<KhoaHoc> lsKhoaHocBySkill = await _khoaHocRepository.GetMultiByListConditionAndAsync(khoaHoccolums);

            List<dynamic> lsVideoKhoaHocLienQuanTheoSkill = new List<dynamic>();

            foreach (KhoaHoc khoaHoc in lsKhoaHocBySkill)
            {
                KhoaHocViewModel khoaHocVm = _mapper.Map<KhoaHocViewModel>(khoaHoc);

                KhoaHocSkillViewModel khoaHocSkillVm = new KhoaHocSkillViewModel();

                khoaHocSkillVm.TenKhoaHoc = khoaHocVm.TenKhoaHoc;
                khoaHocSkillVm.HinhAnh = khoaHocVm.HinhAnh;
                khoaHocSkillVm.SoNgayKichHoat = khoaHocVm.SoNgayKichHoat;
                khoaHocSkillVm.DanhSachChuongHocSkill = new List<dynamic>();

                //Lấy ra danh sách các chương học trong khóa
                List<dynamic> dsChuongHocTrongKhoa = JsonConvert.DeserializeObject<List<dynamic>>(khoaHoc.DanhSachChuongHoc);

                IEnumerable<ChuongHoc> lsChuongHoc = await _chuongHocRepository.GetMultiByIdAsync(dsChuongHocTrongKhoa);

                //Duyệt từng chương học để lấy bài học 
                foreach (ChuongHoc chuongHoc in lsChuongHoc)
                {

                    ChuongHocViewModel chuongHocVm = _mapper.Map<ChuongHocViewModel>(chuongHoc);

                    chuongHocVm.DanhSachBaiHoc = new List<dynamic>();

                    //Lấy ra danh sách bài học trong chương
                    List<dynamic> dsBaiHocTrongChuong = JsonConvert.DeserializeObject<List<dynamic>>(chuongHoc.DanhSachBaiHoc);
                    IEnumerable<BaiHoc> lsBaiHoc = await _baiHocRepository.GetMultiByIdAsync(dsBaiHocTrongChuong);

                    //Chuyển list modal bài học thành list view

                    List<BaiHocViewModel> lsBaiHocVm = _mapper.Map<List<BaiHocViewModel>>(lsBaiHoc);

                    chuongHocVm.DanhSachBaiHoc.Add(lsBaiHocVm);
                    khoaHocSkillVm.DanhSachChuongHocSkill.Add(chuongHocVm);
                }

                lsVideoKhoaHocLienQuanTheoSkill.Add(khoaHocSkillVm);
            }
            return lsVideoKhoaHocLienQuanTheoSkill;
        }

        private int ConvertNgayTrongTuan(int input)
        {
            // thu 2
            if (input == 1 || input == 7 || input == 14)
                return 1;
            // thu 3
            if (input == 2 || input == 8 || input == 15)
                return 2;
            // thu 4
            if (input == 3 || input == 9 || input == 16)
                return 3;
            // thu 5
            if (input == 4 || input == 10 || input == 17)
                return 4;
            // thu 6
            if (input == 5 || input == 11 || input == 18)
                return 5;
            // thu 7
            if (input == 6 || input == 12 || input == 19)
                return 6;
            // CN
            if (input == 13 || input == 20)
                return 0;
            return -1;
        }
    }
}