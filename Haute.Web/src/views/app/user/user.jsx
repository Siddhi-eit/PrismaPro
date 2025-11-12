import React, { useState, useEffect } from 'react';
import useMousetrap from 'hooks/use-mousetrap';
import { connect } from 'react-redux';
import Grid from 'containers/userManagement/Grid';
import PageHeader from 'containers/pages/userManagement/PageHeader';
import UserModal from 'containers/pages/userManagement/UserModal';
import DeleteModal from 'containers/pages/DeleteModal';
import {
  addUser,
  addUserResetState,
  deleteUser,
  getUserList,
  getUserByID,
  bindUsertypeDropdown,
  DownloadUserExcelFile,
} from 'redux/actions';
import createNotification from 'helpers/alerts';

const getIndex = (value, arr, prop) => {
  for (let i = 0; i < arr.length; i += 1) {
    if (arr[i][prop] === value) {
      return i;
    }
  }
  return -1;
};
const orderOptions = [
  { column: '0', label: 'Firstname' },
  { column: '1', label: 'Lastname' },
  { column: '2', label: 'User Name' },
  { column: '3', label: 'Email' },
  { column: '7', label: 'Phone no' },
  { column: '4', label: 'Rolename' },
  { column: '12', label: 'Created Date' },
  { column: '9', label: 'Active?' },
];
const orderDirection = [
  { sortOrder: 'asc', label: 'A-Z' },
  { sortOrder: 'desc', label: 'Z-A' },
];

const pageSizes = [10, 20, 50, 100];
const UserList = ({
  match,
  UserListAction,
  allUserItems,
  UserDeleteAction,
  isSucessfullyDelete,
  UserAddAction,
  isSucessfullyAdd,
  UserAddUserResetStateAction,
  UserGetByIDAction,
  userDetail,
  resultMessage,
  loading,
  UsertypeDropdownAction,
  userTypeDropdownList,
  DownloadUserExcelFileAction,
}) => {
  const [isLoaded, setIsLoaded] = useState(false);
  const [displayMode, setDisplayMode] = useState('thumblist');
  const [currentPage, setCurrentPage] = useState(1);
  const [selectedPageSize, setSelectedPageSize] = useState(10);
  const [selectedOrderOption, setSelectedOrderOption] = useState({
    column: '12',
    label: 'Created Date',
  });
  const [modalOpen, setModalOpen] = useState(false);
  const [totalItemCount, setTotalItemCount] = useState(0);
  const [totalPage, setTotalPage] = useState(1);
  const [search, setSearch] = useState('');
  const [selectedItems, setSelectedItems] = useState([]);
  const [selectedUserID, setSelectedUserID] = useState('');
  const [items, setItems] = useState([]);
  const [lastChecked, setLastChecked] = useState(null);
  const [deleteMessage, setDeleteMessage] = useState(null);
  const [isDeleteModalOpen, seIsDeleteModalOpen] = useState(false);
  const [btnLoader, setbtnLoader] = useState(false);
  const [selectedOrderDirection, setSelectedOrderDirection] = useState({
    sortOrder: 'desc',
    label: 'Z-A',
  });

  useEffect(() => {
    setCurrentPage(1);
  }, [selectedPageSize, selectedOrderOption]);
  useEffect(() => {
    async function fetchData() {
      const gridObject = {
        pageSize: selectedPageSize,
        currentPage,
        orderBy: selectedOrderOption.column,
        search,
        orderDirection: selectedOrderDirection.sortOrder,
      };
      UserListAction(gridObject);
    }
    fetchData();
  }, [
    selectedPageSize,
    currentPage,
    selectedOrderOption,
    selectedOrderDirection,
    search,
    UserListAction,
  ]);
  useEffect(() => {
    if (isSucessfullyAdd > 0) {
      setModalOpen(!modalOpen);
      UserAddUserResetStateAction();
    }
    if (allUserItems != null) {
      setTotalPage(allUserItems.totalPage);
      setItems(
        allUserItems.data.map((x) => {
          return { ...x };
        })
      );
      setSelectedItems([]);
      setTotalItemCount(allUserItems.totalItem);
      setIsLoaded(true);
    }
  }, [
    allUserItems,
    selectedPageSize,
    currentPage,
    selectedOrderOption,
    search,
    isSucessfullyAdd,
    modalOpen,
    UserAddUserResetStateAction,
    selectedOrderDirection,
  ]);

  useEffect(() => {
    if (
      (isSucessfullyDelete !== null &&
        isSucessfullyDelete !== undefined &&
        isSucessfullyDelete !== 0) ||
      (isSucessfullyAdd != null && isSucessfullyAdd > 0)
    ) {
      const gridObject = {
        pageSize: selectedPageSize,
        currentPage,
        orderBy: selectedOrderOption.column,
        search,
        orderDirection: selectedOrderDirection.sortOrder,
      };
      UserListAction(gridObject);
    }
  }, [
    isSucessfullyDelete,
    isSucessfullyAdd,
    selectedPageSize,
    currentPage,
    selectedOrderOption.column,
    search,
    UserListAction,
    selectedOrderDirection,
  ]);

  useEffect(() => {
    if (
      (isSucessfullyDelete !== null &&
        isSucessfullyDelete !== undefined &&
        isSucessfullyDelete !== 0) ||
      (isSucessfullyAdd != null && isSucessfullyAdd > 0)
    ) {
      const gridObject = {
        pageSize: selectedPageSize,
        currentPage,
        orderBy: selectedOrderOption.column,
        search,
        orderDirection: selectedOrderDirection.sortOrder,
      };
      UserListAction(gridObject);
    }
    if (isSucessfullyAdd != null && isSucessfullyAdd > 0) {
      createNotification('success', 'Success', resultMessage);
    }
    if (isSucessfullyAdd != null && isSucessfullyAdd === -1) {
      createNotification('warning', 'Warning', resultMessage);
    }
    if (isSucessfullyAdd != null && isSucessfullyAdd === -2) {
      createNotification('warning', 'Warning', resultMessage);
    }
    if (isSucessfullyAdd != null && isSucessfullyAdd === -3) {
      createNotification('warning', 'Warning', resultMessage);
    }
    if (
      (isSucessfullyDelete !== null || isSucessfullyDelete !== undefined) &&
      isSucessfullyDelete !== 0
    ) {
      createNotification('success', 'Success', 'Deleted Successfully !!');
    }
  }, [isSucessfullyDelete, isSucessfullyAdd]);

  const onCheckItem = (event, id) => {
    if (
      event.target.tagName === 'A' ||
      (event.target.parentElement && event.target.parentElement.tagName === 'A')
    ) {
      return true;
    }
    if (lastChecked === null) {
      setLastChecked(id);
    }
    let selectedList = [...selectedItems];
    if (selectedList.includes(id)) {
      selectedList = selectedList.filter((x) => x !== id);
    } else {
      selectedList.push(id);
    }
    setSelectedItems(selectedList);
    if (event.shiftKey) {
      let newItems = [...items];
      const start = getIndex(id, newItems, 'id');
      const end = getIndex(lastChecked, newItems, 'id');
      newItems = newItems.slice(Math.min(start, end), Math.max(start, end) + 1);
      selectedItems.push(
        ...newItems.map((item) => {
          return item.id;
        })
      );
      selectedList = Array.from(new Set(selectedItems));
      setSelectedItems(selectedList);
    }
    document.activeElement.blur();
    return false;
  };
  const handleChangeSelectAll = (isToggle) => {
    if (selectedItems.length >= items.length) {
      if (isToggle) {
        setSelectedItems([]);
      }
    } else {
      setSelectedItems(items.map((x) => x.id));
    }
    document.activeElement.blur();
    return false;
  };
  const onContextMenuClick = (e, data) => {
    if (data.action === 'delete') {
      setDeleteMessage('Are you sure you want to delete?');
      seIsDeleteModalOpen(!isDeleteModalOpen);
      UserAddUserResetStateAction();
    } else if (data.action === 'Edit') {
      UserGetByIDAction(selectedUserID);
    }
  };
  useEffect(() => {
    if (userDetail != null) {
      setModalOpen(!modalOpen);
    }
  }, [userDetail]);
  const onContextMenu = (e, data) => {
    const clickedProductId = data.data;
    setSelectedUserID(clickedProductId);
    if (!selectedItems.includes(clickedProductId)) {
      setSelectedItems([clickedProductId]);
    }
    return true;
  };
  useMousetrap(['ctrl+a', 'command+a'], () => {
    handleChangeSelectAll(false);
  });
  useMousetrap(['ctrl+d', 'command+d'], () => {
    setSelectedItems([]);
    return false;
  });
  const startIndex = (currentPage - 1) * selectedPageSize;
  const endIndex = currentPage * selectedPageSize;

  const onDeleteClick = () => {
    seIsDeleteModalOpen(!isDeleteModalOpen);
    UserDeleteAction(selectedItems);
    UserAddUserResetStateAction();
  };
  const onUserSubmit = (value) => {
    UserAddAction(value);
    setbtnLoader(true);
  };
  const [machineDropdownItem, setmachineDropdownItem] = useState([]);
  useEffect(() => {
    UsertypeDropdownAction();
  }, []);

  useEffect(() => {
    if (userTypeDropdownList != null && userTypeDropdownList.length > 0) {
      const options = userTypeDropdownList.map((d) => ({
        value: d.id,
        label: d.roleName,
      }));
      setmachineDropdownItem(options);
    }
  }, [userTypeDropdownList]);

  const ExportDispenseData = () => {
    DownloadUserExcelFileAction();
  };

  useEffect(() => {
    document.getElementById('machineDropdown').style.display = 'none';
  }, []);
  return (
    <>
      {loading && (
        <div className="customOverlay">
          <div className="loading" />
        </div>
      )}
      <div className="disable-text-selection">
        <PageHeader
          heading="Users"
          displayMode={displayMode}
          changeDisplayMode={setDisplayMode}
          handleChangeSelectAll={handleChangeSelectAll}
          changeOrderBy={(column) => {
            setSelectedOrderOption(
              orderOptions.find((x) => x.column === column)
            );
          }}
          changePageSize={setSelectedPageSize}
          selectedPageSize={selectedPageSize}
          totalItemCount={totalItemCount}
          selectedOrderOption={selectedOrderOption}
          match={match}
          startIndex={startIndex}
          endIndex={endIndex}
          selectedItemsLength={selectedItems ? selectedItems.length : 0}
          itemsLength={items ? items.length : 0}
          onSearchKey={(e) => {
            if (e.key === 'Enter') {
              setSearch(e.target.value.toLowerCase());
            }
          }}
          orderOptions={orderOptions}
          pageSizes={pageSizes}
          toggleModal={() => {
            setModalOpen(!modalOpen);
            UserAddUserResetStateAction();
          }}
          orderDirection={orderDirection}
          selectedOrderDirection={selectedOrderDirection}
          changeOrderDirection={(sortOrder) => {
            setSelectedOrderDirection(
              orderDirection.find((x) => x.sortOrder === sortOrder)
            );
          }}
          ExportData={() => {
            ExportDispenseData();
          }}
        />
        <UserModal
          modalOpen={modalOpen}
          toggleModal={() => {
            setModalOpen(!modalOpen);
            UserAddUserResetStateAction();
          }}
          categories={machineDropdownItem}
          onUserSubmit={onUserSubmit}
          userDetail={userDetail}
          btnLoader={btnLoader}
        />
        <DeleteModal
          deleteMessage={deleteMessage}
          isDeleteModalOpen={isDeleteModalOpen}
          deletedClick={() => onDeleteClick()}
          toggleModal={() => seIsDeleteModalOpen(!isDeleteModalOpen)}
        />
        <Grid
          items={items}
          allUserItems={allUserItems}
          displayMode={displayMode}
          selectedItems={selectedItems}
          onCheckItem={onCheckItem}
          currentPage={currentPage}
          totalPage={totalPage}
          onContextMenuClick={onContextMenuClick}
          onContextMenu={onContextMenu}
          onChangePage={setCurrentPage}
        />
      </div>
    </>
  );
};

const mapStateToProps = ({ user }) => {
  const {
    allUserItems,
    isSucessfullyDelete,
    isSucessfullyAdd,
    isAddError,
    userDetail,
    resultMessage,
    loading,
    userTypeDropdownList,
  } = user;

  return {
    allUserItems,
    isSucessfullyDelete,
    isSucessfullyAdd,
    isAddError,
    userDetail,
    resultMessage,
    loading,
    userTypeDropdownList,
  };
};
export default connect(mapStateToProps, {
  UserListAction: getUserList,
  UserDeleteAction: deleteUser,
  UserAddAction: addUser,
  UserAddUserResetStateAction: addUserResetState,
  UserGetByIDAction: getUserByID,
  UsertypeDropdownAction: bindUsertypeDropdown,
  DownloadUserExcelFileAction: DownloadUserExcelFile,
})(UserList);
