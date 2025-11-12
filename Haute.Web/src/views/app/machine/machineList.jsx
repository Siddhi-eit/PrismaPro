import React, { useState, useEffect } from 'react';
import useMousetrap from 'hooks/use-mousetrap';
import { connect } from 'react-redux';
import MachineListing from 'containers/machineGrid/MachineListing';
import MachineListPageHeading from 'containers/pages/MachineListPageHeading';
import MachineModal from 'containers/pages/MachineModal';
import DeleteModal from 'containers/pages/DeleteModal';
import {
  addMachine,
  addMachineResetState,
  deleteMachine,
  getMachineList,
  getMachineByID,
  downloadMachineExcelFile,
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
  { column: '2', label: 'MachineRegNo' },
  { column: '3', label: 'ShopName' },
  { column: '4', label: 'ShopAddress' },
  { column: '5', label: 'City' },
  { column: '6', label: 'State' },
  { column: '7', label: 'MacAddress' },
  { column: '8', label: 'CreatedDate' },
  { column: '9', label: 'IsActive' },
];
const orderDirection = [
  { sortOrder: 'asc', label: 'A-Z' },
  { sortOrder: 'desc', label: 'Z-A' },
];

const pageSizes = [10, 20, 50, 100];
const MachineList = ({
  match,
  MachineListAction,
  allMachineItems,
  MachineDeleteAction,
  isSucessfullyDelete,
  MachineAddAction,
  isSucessfullyAdd,
  AddMachineResetStateAction,
  MachineGetByIDAction,
  machineDetail,
  loading,
  machineID,
  resultMessage,
  DownloadMachineExcelFileAction,
}) => {
  const [isLoaded, setIsLoaded] = useState(false);
  const [displayMode, setDisplayMode] = useState('thumblist');
  const [currentPage, setCurrentPage] = useState(1);
  const [selectedPageSize, setSelectedPageSize] = useState(10);
  const [selectedOrderOption, setSelectedOrderOption] = useState({
    column: '6',
    label: 'Created Date',
  });
  const [modalOpen, setModalOpen] = useState(false);
  const [totalItemCount, setTotalItemCount] = useState(0);
  const [totalPage, setTotalPage] = useState(1);
  const [search, setSearch] = useState('');
  const [selectedItems, setSelectedItems] = useState([]);
  const [selectedMachineID, setSelectedMachineID] = useState('');
  const [items, setItems] = useState([]);
  const [lastChecked, setLastChecked] = useState(null);
  const [deleteMessage, setDeleteMessage] = useState(null);
  const [isDeleteModalOpen, seIsDeleteModalOpen] = useState(false);
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
        machineID: machineID,
      };
      MachineListAction(gridObject);
    }
    fetchData();
  }, [
    selectedPageSize,
    currentPage,
    selectedOrderOption,
    search,
    machineID,
    selectedOrderDirection,
  ]);
  useEffect(() => {
    if (isSucessfullyAdd != null && isSucessfullyAdd > 0) {
      setModalOpen(!modalOpen);
      AddMachineResetStateAction();
    }
    if (allMachineItems != null) {
      setTotalPage(allMachineItems.totalPage);
      setItems(
        allMachineItems.data.map((x) => {
          return { ...x };
        })
      );
      setSelectedItems([]);
      setTotalItemCount(allMachineItems.totalItem);
      setIsLoaded(true);
    }
  }, [
    allMachineItems,
    selectedPageSize,
    currentPage,
    selectedOrderOption,
    search,
    isSucessfullyAdd,
    selectedOrderDirection,
  ]);

  useEffect(() => {
    if (
      (isSucessfullyDelete !== null &&
        isSucessfullyDelete !== undefined &&
        isSucessfullyDelete !== 0) ||
      (isSucessfullyAdd != null &&
        isSucessfullyAdd > 0 &&
        isSucessfullyAdd === -1)
    ) {
      const gridObject = {
        pageSize: selectedPageSize,
        currentPage,
        orderBy: selectedOrderOption.column,
        search,
        orderDirection: selectedOrderDirection.sortOrder,
        machineID: machineID,
      };
      MachineListAction(gridObject);
    }
    if (isSucessfullyAdd === null && isSucessfullyAdd < 0) {
      createNotification('error', 'Error', resultMessage);
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
      AddMachineResetStateAction();
    } else if (data.action === 'Edit') {
      MachineGetByIDAction(selectedMachineID);
    }
  };
  useEffect(() => {
    if (machineDetail != null) {
      setModalOpen(!modalOpen);
    }
  }, [machineDetail]);
  const onContextMenu = (e, data) => {
    const clickedProductId = data.data;
    setSelectedMachineID(clickedProductId);
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
    MachineDeleteAction(selectedMachineID);
  };
  const onMachineSubmit = (value) => {
    debugger;
    MachineAddAction(value);
  };

  const ExportMachineData = () => {
    DownloadMachineExcelFileAction();
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
        <MachineListPageHeading
          heading="Machine"
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
            AddMachineResetStateAction();
          }}
          orderDirection={orderDirection}
          selectedOrderDirection={selectedOrderDirection}
          changeOrderDirection={(sortOrder) => {
            setSelectedOrderDirection(
              orderDirection.find((x) => x.sortOrder === sortOrder)
            );
          }}
          ExportData={() => {
            ExportMachineData();
          }}
        />
        <MachineModal
          modalOpen={modalOpen}
          toggleModal={() => {
            setModalOpen(!modalOpen);
            AddMachineResetStateAction();
          }}
          onMachineSubmit={onMachineSubmit}
          machineDetail={machineDetail}
        />
        <DeleteModal
          deleteMessage={deleteMessage}
          isDeleteModalOpen={isDeleteModalOpen}
          deletedClick={() => onDeleteClick()}
          toggleModal={() => seIsDeleteModalOpen(!isDeleteModalOpen)}
        />
        <MachineListing
          items={items}
          allMachineItems={allMachineItems}
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

const mapStateToProps = ({ machine, user }) => {
  const {
    allMachineItems,
    isSucessfullyDelete,
    isSucessfullyAdd,
    isAddError,
    machineDetail,
    loading,
    dispenseUnitDropdownList,
    resultMessage,
  } = machine;
  const { machineID } = user;
  return {
    allMachineItems,
    isSucessfullyDelete,
    isSucessfullyAdd,
    isAddError,
    machineDetail,
    loading,
    machineID,
    dispenseUnitDropdownList,
    resultMessage,
  };
};
export default connect(mapStateToProps, {
  MachineDeleteAction: deleteMachine,
  MachineAddAction: addMachine,
  AddMachineResetStateAction: addMachineResetState,
  MachineGetByIDAction: getMachineByID,
  MachineListAction: getMachineList,
  DownloadMachineExcelFileAction: downloadMachineExcelFile,
})(MachineList);
